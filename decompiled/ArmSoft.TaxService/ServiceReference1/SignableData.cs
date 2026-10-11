using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[XmlInclude(typeof(BatchResponse))]
[XmlInclude(typeof(InvoiceNumberBatch))]
[XmlInclude(typeof(InvoiceNumberRequest))]
[XmlInclude(typeof(AccDocInvalidationRequest))]
[XmlInclude(typeof(InvalidationRequest))]
[XmlInclude(typeof(InvalidationBatch))]
[XmlInclude(typeof(InvoiceBatch))]
[XmlInclude(typeof(AccDocBatch))]
[XmlInclude(typeof(AccountingDocument))]
[XmlInclude(typeof(BookDecl))]
[XmlInclude(typeof(Invoice))]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class SignableData
{
}
