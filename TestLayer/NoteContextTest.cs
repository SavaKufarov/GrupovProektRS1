using DataLayer;
using BusinessLayer;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace TestLayer
{
    [TestFixture]
    public class NoteContextTest
    {
        static NoteContext noteContext;
        static User testUser;
        static Expense testExpense;

        static NoteContextTest()
        {
            noteContext = new NoteContext(TestManager.dbContext);

            var categoryContext = new CategoryContext(TestManager.dbContext);
            var testCategory = new Category("Test Category");
            categoryContext.Create(testCategory);

            var expenseContext = new ExpenseContext(TestManager.dbContext);
            testExpense = new Expense()
            {
                Name = "Test Expense",
                Amount = 50.00m,
                CategoryId = testCategory.Id
            };
            expenseContext.Create(testExpense);
        }

        [Test]
        public void CreateNote()
        {
            Note note = new Note()
            {
                Budget = 100.00m,
                Description = "Test Note",
                User = testUser,
                Expense = testExpense
            };

            int notesBefore = TestManager.dbContext.Notes.Count();
            noteContext.Create(note);
            int notesAfter = TestManager.dbContext.Notes.Count();
            Note lastNote = TestManager.dbContext.Notes.Last();

            Assert.That(notesBefore + 1 == notesAfter && lastNote.Budget == note.Budget,
                "No such note or Budget does not match!");
        }

        [Test]
        public void DeleteNote()
        {
            Note newNote = new Note()
            {
                Budget = 200.00m,
                Description = "Note to delete",
                User = testUser,
                Expense = testExpense
            };
            noteContext.Create(newNote);

            List<Note> notes = noteContext.ReadAll();
            int notesBefore = notes.Count;
            Note noteToDelete = notes.Last();

            noteContext.Delete(noteToDelete.Id);

            int notesAfter = noteContext.ReadAll().Count;
            Assert.That(notesBefore == notesAfter + 1, "Delete() does not delete a note!");
        }

        [Test]
        public void ReadNote()
        {
            Note newNote = new Note()
            {
                Budget = 150.00m,
                Description = "Test Read Note",
                User = testUser,
                Expense = testExpense
            };
            noteContext.Create(newNote);

            Note retrievedNote = noteContext.Read(newNote.Id);

            Assert.That(retrievedNote.Description == "Test Read Note",
                "Read() does not get note by id!");
        }

        [Test]
        public void ReadAllNotes()
        {
            int notesBefore = TestManager.dbContext.Notes.Count();
            int notesAfter = noteContext.ReadAll().Count;
            Assert.That(notesBefore == notesAfter, "ReadAll() does not return all of the notes!");
        }

        [Test]
        public void UpdateNote()
        {
            Note newNote = new Note()
            {
                Budget = 300.00m,
                Description = "Original Description",
                User = testUser,
                Expense = testExpense
            };
            noteContext.Create(newNote);

            Note lastNote = noteContext.ReadAll().Last();
            lastNote.Description = "Updated Description";

            noteContext.Update(lastNote, false);

            Assert.That(noteContext.Read(lastNote.Id).Description == "Updated Description",
                "Update() does not change the note's description!");
        }
    }
}