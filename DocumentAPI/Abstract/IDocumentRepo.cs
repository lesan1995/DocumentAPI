using DocumentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAPI.Abstract
{
    public interface IDocumentRepo
    {
        List<Document> GetDocuments(DocumentFilter filter);
        Document GetDocument(int documentId);
        bool IsDocumentExist(int documentId);
        Document AddDocument(Document document);
        Document UpdateDocument(Document document);
        Document RemoveDocument(Document document);
    }
}
