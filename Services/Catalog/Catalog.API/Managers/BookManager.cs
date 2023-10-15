﻿using Catalog.Entities;
using Catalog.Managers.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Managers
{
    public class BookManager : GenericManager<Book>, IBookManager
    {
        public BookManager(MongoDbContext context) : base(context)
        {
        }

        public async Task<Book> GetBookByTitle(string title)
        {
            return await _collection
                          .Find(p => p.Title == title)
                          .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthor(string authorName)
        {
            return await _collection
                          .Find(p => p.AuthorName == authorName)
                          .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenre(string genre)
        {
            return await _collection
                          .Find(p => p.Genre == genre)
                          .ToListAsync();
        }

        public async Task<Book> GetBookById(string id)
        {
            return await _collection
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByPublisher(string publisherName)
        {
            return await _collection
                          .Find(p => p.PublisherName == publisherName)
                          .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
