using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace QrBarCodeTestUtility.Models
{
    public class FormExtract
    {
        public string formId { get; set; }
        public string formName { get; set; }
        public string version { get; set; }
        public int dueDay { get; set; }
        public long formVersionId { get; set; }
        public string pdfFileKey { get; set; }
        public String resourceFilesHost { get; set; }
        public int pdfResourceFileId { get; set; }
        public List<FormExtractField> fields { get; set; }
        public List<FormExtractConstant> constants { get; set; }
        public HashSet<String> declarativeImportPaths { get; set; }
        public RepeatGroups repeatGroups { get; set; }
        public SortedDictionary<int, string> omitPageExpressions { get; set; }
        public List<PositionConfiguration> PositionConfigurations { get; set; }
        public FormMasterModel formMaster { get; set; }
        public List<SortDirective> SortDirectives { get; set; }
    }

    public class FormExtractField
    {
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string rounding { get; set; }
        public FieldInputMapping inputDataMapping { get; set; }
        public BulkReview bulkReview { get; set; }
        public FieldPdfExport pdfExport { get; set; }
        public string @default { get; set; }
        public string formula { get; set; }
        public int sortOrder { get; set; }
        public bool ReadOnly { get; set; }
        public Validation validation { get; set; }
        public bool Recalculate { get; set; }
        public bool NoFieldReference { get; set; }
        public bool SkipCalculation { get; set; }
    }

    public class FormExtractConstant
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
        public bool Hidden { get; set; }
    }

    public class FieldInputMapping
    {
        public string deductionTypeIds { get; set; }
        public string taxTypeIds { get; set; }
        public string rateTypeIds { get; set; }
        public string groupCode { get; set; }
        public string reportLevel { get; set; }
        public string fieldType { get; set; }
    }

    public class BulkReview
    {
        public string validationTag { get; set; }
    }

    public class FieldPdfExport
    {
        public string fieldName { get; set; }
        public string pageNumbers { get; set; }
        public bool? emptyIfValueIsZero { get; set; }
        public bool removeDecimal { get; set; }
        public string format { get; set; }
        public string negativeFormat { get; set; }
        public Int64 decimalFormat { get; set; }
    }

    public class RepeatGroups : SortedDictionary<int, RepeatGroup>
    {
    }

    public class RepeatGroup
    {
        public int pageNumber { get; set; }
        public int instances { get; set; }
        public int minPageRow { get; set; }
        public int maxPageRow { get; set; }
        public int sortOrderStride { get; set; }
        public string maxRowExpression { get; set; }
        public int manualInstances { get; set; }
        public List<Int32> pagesInSet { get; set; }
        public SortedDictionary<string, RepeatRange> repeatRanges { get; set; }
    }

    public class RepeatRange
    {
        public string pdfPrefix { get; set; }
        public string fieldPrefix { get; set; }
        public int? min { get; set; }
        public int? max { get; set; }
        public int? stride { get; set; }
    }

    public class PositionConfiguration
    {
        /* position column name and target table */
        public String PositionColumnName { get; set; }
        public String InputDataTableName { get; set; }

        /* ordering */
        public String FixedPositionColumn { get; set; }
        public List<String> FixedPositionValues { get; set; }
        public List<OrderByColumn> OrderByColumns { get; set; }

        /* filters */
        public List<String> HideIfZeroColumns { get; set; }
        public List<ValueIn> InFilters { get; set; }
    }

    public class ValueIn
    {
        public String InFilterColumn { get; set; }
        public Boolean Negate { get; set; }
        public List<String> InFilterValues { get; set; }
    }

    public class OrderByColumn
    {
        public String ColumnName { get; set; }
        public String Direction { get; set; }
        public Int32? TruncatedLength { get; set; }
        public JObject Conversions { get; set; }
    }

    public class Validation
    {
        public ValidationErrorSeverity Severity { get; set; }
        public String Message { get; set; }
        public String Expression { get; set; }
    }

    public class SortDirective
    {
        public String Name { get; set; }
        public List<String> Suffixes { get; set; }
        public List<IncludeColumn> IncludeColumns { get; set; }
    }

    public class IncludeColumn
    {
        public String FieldPrefix { get; set; }
        public String Direction { get; set; }
        public Boolean BlanksAndZerosLast { get; set; }
    }
}
