using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        private bool isEditing;

        public bool IsEditing
        {
            get { return isEditing; }
            set
            {
                isEditing = value;
                OnPropertyChanged("IsEditing");
            }
        }

        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");
                if (selectedNotebook != null)
                    ReadNotes();
            }
        }

        private Note note;

        public Note SelectedNote
        {
            get { return note; }
            set
            {
                note = value;
                SelectedNoteChanged(this, new EventArgs());
            }
        }


        public ObservableCollection<Note> Notes { get; set; }

        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public BeginEditCommand BeginEditCommand { get; set; }
        public HasEditedCommand HasEditedCommand { get; set; }
        public DeleteNotebookCommand DeleteNotebookCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        public NotesVM()
        {
            IsEditing = false;

            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            BeginEditCommand = new BeginEditCommand(this);
            HasEditedCommand = new HasEditedCommand(this);
            DeleteNotebookCommand = new DeleteNotebookCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            ReadNotebooks();
            ReadNotes();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New notebook",
                UserId = App.UserId
            };

            await DatabaseHelper.Insert(newNotebook);

            ReadNotebooks();
        }

        public async void DeleteNotebook(Notebook notebook)
        {
            await DatabaseHelper.Delete(notebook);
            Notebooks.Remove(notebook);
            // ReadNotebooks();
        }

        public async void CreateNote(string notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Title = "New note"
            };

            await DatabaseHelper.Insert(newNote);

            ReadNotes();
        }

        public async void ReadNotebooks()
        {
            //using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            //{
            //    conn.CreateTable<Notebook>();
            //    var notebooks = conn.Table<Notebook>().ToList();

            //    Notebooks.Clear();
            //    foreach (var notebook in notebooks)
            //    {
            //        Notebooks.Add(notebook);
            //    }
            //}

            var notebooks = await DatabaseHelper.Read<Notebook>();
            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }

        public async void ReadNotes()
        {
            //using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            //{
            //    conn.CreateTable<Note>();
            //    if (SelectedNotebook != null)
            //    {
            //        var notes = conn.Table<Note>().Where(n => n.NotebookId == SelectedNotebook.Id).ToList();

            //        Notes.Clear();
            //        foreach(var note in notes)
            //        {
            //            Notes.Add(note);
            //        }
            //    }
            //}

            var notes = await DatabaseHelper.Read<Note>();
            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }

        public void StartEditing()
        {
            IsEditing = true;
        }

        public async void HasRenamed(Notebook notebook)
        {
            if(notebook != null)
            {
                await DatabaseHelper.Update(notebook);
                IsEditing = false;
                ReadNotebooks();
            }
        }

        public async void UpdateSelectedNote()
        {
            await DatabaseHelper.Update(SelectedNote);
        }
    }
}
