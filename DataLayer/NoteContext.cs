using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;

namespace DataLayer
{

        public class NoteContext : IDb<Note, int>
        {
            private ExpenseTrackerDbContext dbContext;
            public NoteContext(ExpenseTrackerDbContext appContext)
            {
                dbContext = appContext;
            }
            public void Create(Note item)
            {
                dbContext.Notes.Add(item);
                dbContext.SaveChanges();
            }
            public Note Read(int id, bool useNavigationalProperties = false, bool isReadOnly = false)
            {
                IQueryable<Note> query = dbContext.Notes;
                if (useNavigationalProperties)
                {

                }
                if (isReadOnly)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }
                Note note = query.FirstOrDefault(f => f.Id == id);
                if (note == null)
                {
                    throw new Exception("Note not found!");
                }
                return note;
            }
            public List<Note> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
            {
                IQueryable<Note> query = dbContext.Notes;
                if (useNavigationalProperties)
                {

                }
                if (isReadOnly)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }
                return query.ToList();
            }
            public void Update(Note note, bool useNavigationalProperties = false)
            {
                Note noteFromContext = dbContext.Notes.Find(note.Id);
                noteFromContext.Id = note.Id;

                dbContext.SaveChanges();
            }
            public void Delete(int id)
            {
                Note note = dbContext.Notes.Find(id);
                if (note != null)
                {
                    dbContext.Notes.Remove(note);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Note not found!");
                }
            }
        }
    
}
