using Dapper;
using DocumentAPI.Abstract;
using DocumentAPI.DAL;
using DocumentAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAPI.Concrete
{
    public class DocumentRepo : IDocumentRepo
    {
        private readonly string _connectionString;
        private readonly DocumentDbContext _context;
        public DocumentRepo(
            string connectionString,
            DocumentDbContext context
            )
        {
            _connectionString = connectionString;
            _context = context;
        }

        public List<Document> GetDocuments(DocumentFilter filter)
        {
            using (var context = new SqlConnection(_connectionString))
            {
                //Get list document
                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("SELECT Id ");
                sbSelect.Append("      ,CategoryId ");
                sbSelect.Append("      ,Title ");
                sbSelect.Append("      ,Description ");
                sbSelect.Append("      ,Cover ");
                sbSelect.Append("      ,PublishYear ");
                sbSelect.Append("  FROM Documents ");
                sbSelect.Append("  WHERE Title Collate Latin1_General_CI_AI LIKE N'%'+@keyword+'%' ");
                if (filter.CategoryId.HasValue)
                {
                    sbSelect.Append("  AND CategoryId = @categoryId ");
                }
                sbSelect.Append("  ORDER BY Id DESC  ");

                string querySelect = sbSelect.ToString();

                List<Document> documents = context.Query<Document>(querySelect, new {
                    keyword = filter.Keyword ?? string.Empty,
                    categoryId = filter.CategoryId
                }).ToList();

                return documents;
            }
        }

        public Document GetDocument(int documentId)
        {
            return _context.Documents.FirstOrDefault(x => x.Id == documentId);
        }

        public bool IsDocumentExist(int documentId)
        {
            return _context.Documents.Any(x => x.Id == documentId);
        }

        public Document AddDocument(Document document)
        {
            var result = _context.Documents.Add(document).Entity;
            _context.SaveChanges();
            return result;
        }

        public Document UpdateDocument(Document document)
        {
            _context.Entry(document).State = EntityState.Modified;
            _context.SaveChanges();
            return document;
        }

        public Document RemoveDocument(Document document)
        {
            _context.Documents.Remove(document);
            _context.SaveChanges();
            return document;
        }
    }
}
