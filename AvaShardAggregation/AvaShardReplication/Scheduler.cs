using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace AvaShardReplication
{
        /// <summary>
        /// Scheduler Class 
        /// </summary>
        public class Scheduler : SchedulerBase, IDisposable
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public Scheduler()
                : base()
            {
            }

            ~Scheduler()
            {
                Dispose(false);
            }

            protected override void RefreshSiteSettingsCache()
            {
                _cacheTtlHours = ParseSettingValue(DataService.SiteSettings.GetSiteSettingValue("DATASTORE_CACHE_TTL_HOURS", ZyTax.ZMS.Common.Contracts.Indicators.GovernmentFiler.Both), 3);

                //check for old datastores here, its faster not to check everytime, and the heart beat is often enough
                DateTime startDate = DateTime.UtcNow.AddHours(-_cacheTtlHours);
                ZyTax.ZMS.Common.DataStore.MasterDataStore.ClearOldStores(startDate);

                if (ScheduledTaskMonitor != null)
                {
                    ScheduledTaskMonitor.RefreshSettings();
                }

                if (WorkflowMonitor != null)
                {
                    WorkflowMonitor.RefreshSettings();
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            protected override ICancellableTask[] StartRecurringTasks(CancellationTokenSource cancelToken, BlockingCollection<LogMessageEventArgs> messages, ManualResetEventSlim databaseErrorResolvedEvent)
            {
                List<ScheduledTaskQueueItemDC> scheduledTaskQueueItems = new List<ScheduledTaskQueueItemDC>();
                List<WorkflowQueueItemDC> workflowQueueItems = new List<WorkflowQueueItemDC>();

                //Initialize Queue by setting any "running" items to cancelled.  These "running" items are in a failed state as there is no worker item actually running them
                #region [ Cancel-Reset Any "Running" Items ]

                try
                {
                    //Workflow: Cancel "Running" Items                
                    workflowQueueItems = DataService.WorkflowQueue.GetWorkflowQueueItemListByStatus(Indicators.Status.Running, System.Environment.MachineName);

                    if (workflowQueueItems != null)
                    {
                        for (int i = 0; i < workflowQueueItems.Count; i++)
                        {
                            WorkflowQueueItemDC item = workflowQueueItems[i];
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            System.Console.WriteLine(Workflow.CancellingRunning, item.WorkflowQueueId.ToString(CultureInfo.InvariantCulture));

                            //Set Status
                            item.StatusInd = Indicators.Status.Canceled;
                            item.ProcessingCompletedDate = DateTime.UtcNow;
                            DataService.WorkflowQueue.UpdateWorkflowQueueItem(ref item, Indicators.Status.Running);
                            ZmsLogger.LogMessage("WorkflowEngine.Scheduler", "Workflow Engine[" + item.WorkflowQueueId + "]: Cancelling of Workflow Queue Item ", "Cancelling Workflow processing request due to Workflow Engine Service initialization. Please resubmit.");
                        }
                    }

                    //Workflow: Cancel "Initialized" Items
                    workflowQueueItems = DataService.WorkflowQueue.GetWorkflowQueueItemListByStatus(Indicators.Status.Initialized, System.Environment.MachineName);

                    if (workflowQueueItems != null)
                    {
                        for (int i = 0; i < workflowQueueItems.Count; i++)
                        {
                            WorkflowQueueItemDC item = workflowQueueItems[i];
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            System.Console.WriteLine(Workflow.CancellingInitialized, item.WorkflowQueueId.ToString(CultureInfo.InvariantCulture));

                            //Set Status
                            item.StatusInd = Indicators.Status.Canceled;
                            item.ProcessingCompletedDate = DateTime.UtcNow;
                            DataService.WorkflowQueue.UpdateWorkflowQueueItem(ref item, Indicators.Status.Initialized);
                            ZmsLogger.LogMessage("WorkflowEngine.Scheduler", "Workflow Engine[" + item.WorkflowQueueId + "]: Cancelling of Workflow Queue Item ", "Cancelling  processing request due to Workflow Engine Service initialization. Please resubmit.");
                        }
                    }

                    //Scheduled Task: Reset "Running" Items                
                    scheduledTaskQueueItems = DataService.ScheduledTaskQueue.GetScheduledTaskQueueItemListByFilter(null, null, Indicators.Status.Running, null, null, null, null, null, null, null, null, null, null, System.Environment.MachineName, null, null, null, null, null, null);
                    if (scheduledTaskQueueItems != null)
                    {
                        for (int i = 0; i < scheduledTaskQueueItems.Count; i++)
                        {
                            ScheduledTaskQueueItemDC item = scheduledTaskQueueItems[i];

                            DateTime? prevStartedDate = item.ProcessingStartedDate;

                            //ZMS-25644 JMH If the Process Accounting Transactions task is in a running status, send an email and stop. The connection to the database was likely lost or the Workflow Engine was improperly shut down
                            if (item.ScheduledTaskTypeId == Convert.ToInt32(Types.ScheduledTaskType.ProcessAccountingTransactions, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                string subject = "Process Accounting Transactions Task - Failure";
                                string messageBody = "When the workflow service started, the Process Accounting Transaction task was already in a Running status. This indicates that either the database connection was lost or the workflow service was improperly shut down. The process has been cancelled and the task will have a status of Errors. Reset the task by clicking Update under Admin > Scheduled Tasks > Process Accounting Transactions.";
                                SendEmail(item, subject, messageBody);
                                ZyTax.ZMS.Common.ExceptionHandling.Exceptions.ZmsException ex = new ZyTax.ZMS.Common.ExceptionHandling.Exceptions.ZmsException(messageBody);
                                ZmsExceptionHandler.HandleException(ex, "Workflow Engine: Schedule Task Id [" + item.ScheduledTaskQueueId + "]", messageBody);
                                //do not re-throw the error as we do not want to disrupt the service

                                item.StatusInd = Indicators.Status.Errors;
                                item.CompletedDate = DateTime.UtcNow;
                                item.ProcessingCompletedDate = DateTime.UtcNow;
                                DataService.ScheduledTaskQueue.UpdateScheduledTaskQueueItem(ref item, ref prevStartedDate, Indicators.Status.Running);

                                FailSystemActivityStatus(item, ex.Message);
                            }
                            else
                            {
                                System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                                System.Console.WriteLine(Workflow.RestartingRunning, item.ScheduledTaskQueueId.ToString(CultureInfo.InvariantCulture));

                                //ReSet Status
                                item.StatusInd = Indicators.Status.NotStarted;
                                item.ProcessingStartedDate = null;
                                item.ProcessingCompletedDate = null;
                                item.MachineName = null;
                                DataService.ScheduledTaskQueue.UpdateScheduledTaskQueueItem(ref item, ref prevStartedDate, Indicators.Status.Running);
                                ZmsLogger.LogMessage("WorkflowEngine.Scheduler", "Workflow Engine[" + item.ScheduledTaskQueueId + "]: Resetting of Scheduled Task Queue Item ", "Resetting scheduled task due to Workflow Engine Service initialization. Please resubmit.");

                                //Sync any other tables waiting for this job such as system activity
                                SynchronizeStatus(item);
                            }
                        }
                    }

                    //Scheduled Task: Reset "Initialized" Items                
                    scheduledTaskQueueItems = DataService.ScheduledTaskQueue.GetScheduledTaskQueueItemListByFilter(null, null, Indicators.Status.Initialized, null, null, null, null, null, null, null, null, null, null, System.Environment.MachineName, null, null, null, null, null, null);
                    if (scheduledTaskQueueItems != null)
                    {
                        for (int i = 0; i < scheduledTaskQueueItems.Count; i++)
                        {
                            ScheduledTaskQueueItemDC item = scheduledTaskQueueItems[i];
                            DateTime? prevStartedDate = item.ProcessingStartedDate;

                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            System.Console.WriteLine(Workflow.RestartingInitialized, item.ScheduledTaskQueueId.ToString(CultureInfo.InvariantCulture));

                            //Set Status
                            item.StatusInd = Indicators.Status.NotStarted;
                            item.ProcessingStartedDate = null;
                            item.ProcessingCompletedDate = null;
                            item.MachineName = null;
                            DataService.ScheduledTaskQueue.UpdateScheduledTaskQueueItem(ref item, ref prevStartedDate, Indicators.Status.Initialized);
                            ZmsLogger.LogMessage("WorkflowEngine.Scheduler", "Workflow Engine[" + item.ScheduledTaskQueueId + "]: Resetting of Scheduled Task Queue Item ", "Resetting scheduled task due to Workflow Engine Service initialization. Please resubmit.");

                            //Sync any other tables waiting for this job such as system activity
                            SynchronizeStatus(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Continue processing if error occurs.  
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(Workflow.ErrorInitializing);
                    ZmsExceptionHandler.HandleException(ex, "Workflow Engine:Service Starting", "Error Initializing the Workflow Queue Items.  Existing 'Running' and 'Initialized' queue items failed to be set to cancelled.");
                }

                #endregion

                ScheduledTaskMonitor = new ScheduledTaskQueueMonitor(DataService, databaseErrorResolvedEvent);
                ScheduledTaskMonitor.OnError += HandleError;
                ScheduledTaskMonitor.OnLog += (sender, args) => { messages.Add(args); };
                ScheduledTaskMonitor.StartMonitor();

                WorkflowMonitor = new WorkflowQueueMonitor(DataService, databaseErrorResolvedEvent);
                WorkflowMonitor.OnError += HandleError;
                WorkflowMonitor.OnLog += (sender, args) => { messages.Add(args); };
                WorkflowMonitor.StartMonitor();

                return new ICancellableTask[] { WorkflowMonitor, ScheduledTaskMonitor };
            }

            #region [ Helpers ]

            private Int32 ParseSettingValue(String settingValue, Int32 defaultValue)
            {
                Int32 value;
                if (!Int32.TryParse(settingValue, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                {
                    value = defaultValue;
                }

                return value;
            }

            /// <summary>
            /// Based upon the task type, sync up any other tables based upon scheduled task type
            /// note not, every schedule task type has a system activity job id
            /// </summary>
            /// <param name="item"></param>
            private void SynchronizeStatus(ScheduledTaskQueueItemDC item)
            {
                if (item.JobId.HasValue)
                {
                    DataService dataService = new DataService(null);
                    CompanyDC company = dataService.Companies.GetCompanyByPK(item.CompanyId);
                    dataService = new DataService(company.DbName);

                    //Update the system activity
                    ProcessHeaderDC processHeader = dataService.ProcessHeaders.GetProcessHeaderByPK(item.JobId.Value);
                    if (processHeader != null)
                    {
                        if (item.FrequencyInd == Indicators.TaskFrequency.OneTime || item.FrequencyInd == Indicators.TaskFrequency.Custom)
                        {
                            processHeader.StatusInd = item.StatusInd;
                        }
                        else
                        {
                            processHeader.StatusInd = Indicators.Status.Canceled;
                        }

                        processHeader.Action = BusinessObjectBase.ModificationAction.Update;
                        dataService.ProcessHeaders.ModifyProcessHeader(ref processHeader);

                        //Log the reset to system activity
                        dataService.ProcessLogs.WriteProcessLog(processHeader.JobId, Indicators.ErrorLevel.Critical, "Task has been reset due to the workflow service being restarted.", Indicators.Bookmark.None, null);
                    }
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (ScheduledTaskMonitor != null)
                    {
                        ScheduledTaskMonitor.Dispose();
                    }
                }
            }

            #endregion

            #region [ Variables ]
            protected override string ServiceName
            {
                get { return "Workflow Engine"; }
            }

            protected override TimeSpan DelayCloseWaitTime
            {
                get
                {
                    return ScheduledTaskMonitor != null ? ScheduledTaskMonitor.CurrentInterval : TimeSpan.FromSeconds(10);
                }
            }
            private Int32 _cacheTtlHours = 3;

            private WorkflowQueueMonitor WorkflowMonitor;
            private ScheduledTaskQueueMonitor ScheduledTaskMonitor;

            protected void SendEmail(ScheduledTaskQueueItemDC item, string subject, string messageBody)
            {
                MessageDC message = new MessageDC();
                message.SenderCompanyId = item.CompanyId;
                message.MessageSender = "*system*";
                message.MessageDate = DateTime.UtcNow;
                message.MessageSubject = subject;
                message.MessageBody = messageBody;
                message.MessageRecipients = new List<MessageRecipientDC>();
                MessageRecipientDC recipient = new MessageRecipientDC();
                CompanyDC company = DataService.Companies.GetCompanyByPK(item.CompanyId);
                DataService dataService = new DataService(company.DbName);
                List<CompanyInformationDC> companyInfos = dataService.CompanyInformation.GetCompanyInformationListByCompanyId(item.CompanyId);
                if (companyInfos != null && companyInfos.Count > 0)
                {
                    recipient.MessageRecipient = companyInfos[0].CompanyContactUserName;
                }
                else
                {
                    recipient.MessageRecipient = "*system*";
                }

                recipient.RecipientCompanyId = company.MasterCompanyId;
                message.MessageRecipients.Add(recipient);

                //Send email to the user who created the task
                recipient = new MessageRecipientDC();
                recipient.MessageRecipient = item.LoweredUserName;
                recipient.RecipientCompanyId = company.MasterCompanyId;
                message.MessageRecipients.Add(recipient);

                message.MessageTemplateType = "S";
                DataService.MessageCenter.Send(message, true, false);

                //Send email to the email address in the site setting (sending with both recipients and email addresses cannot be done at the same time)
                //Send looks up email addresses for users. In this instance, we have only the email address, not a user id.
                SiteSettingDC emailAddress = dataService.SiteSettings.GetSiteSettingByPK("SYSTEM_ADMINISTRATOR_EMAIL_ADDRESS", Indicators.GovernmentFiler.Both);
                if (!String.IsNullOrEmpty(emailAddress.SettingValue))
                {
                    DataService.MessageCenter.SendEmail(message, emailAddress.SettingValue);
                }
            }

            /// <summary>
            /// When a scheduled task fails badly, we might need to run to the system activity log and reflect the failure as well
            /// </summary>
            /// <param name="errorMessage"></param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            private void FailSystemActivityStatus(ScheduledTaskQueueItemDC item, string errorMessage)
            {
                try
                {
                    //we are in a failure state, so assume nothing and watch out for nulls
                    //if there is found to be a process header, then fail it
                    if (item != null && item.JobId.HasValue && item.JobId > 0)
                    {
                        CompanyDC company = DataService.Companies.GetCompanyByPK(item.CompanyId);
                        DataService dataService = new DataService(company.DbName);
                        ProcessHeaderDC processHeader = dataService.ProcessHeaders.GetProcessHeaderByPK(item.JobId.Value);
                        processHeader.Action = BusinessObjectBase.ModificationAction.Update;
                        processHeader.StatusInd = Indicators.Status.Canceled;
                        dataService.ProcessHeaders.ModifyProcessHeader(ref processHeader);

                        dataService.ProcessLogs.WriteProcessLog(item.JobId.Value, Indicators.ErrorLevel.Critical, "Error running task [" + errorMessage + "]", Indicators.Bookmark.None, null);
                    }
                }
                catch (Exception ex)
                {
                    ZmsExceptionHandler.HandleException(ex, "Workflow Engine: Schedule Task Id [" + item.ScheduledTaskQueueId + "] Fail System Activity Status",
                        "Error Processing of Scheduled Task Queue Item. For Scheduled Task Type [" + item.ScheduledTaskTypeId + "].  Failed to set the system activity status.");
                }
            }

            #endregion
        }
    }

}
