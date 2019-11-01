sample arguments:  "filepath and name" url user pwd formmasterid

example to delete all FormCellDependency records from USAL9501 formmaster:
AddFormCellDependenciesFromExcel "d" https://taxformcatalog-sandbox.returns.avalara.net user password 62

example to pull values from excel file and upload to USAL9501 formmaster:
AddFormCellDependenciesFromExcel "c:\temp\Al9501_CrossCellDepends_v1.xlsx" https://taxformcatalog-sandbox.returns.avalara.net user password 62
