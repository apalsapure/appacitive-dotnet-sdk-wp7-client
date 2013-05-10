using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Appacitive.Mobile.WindowsPhone7.Todo.ViewModels
{
    public class ListItemViewModel : Sdk.Article
    {
        public const string TYPE = "Tasks";


        public ListItemViewModel(Sdk.Article article)
            : base(article)
        {

        }
        public ListItemViewModel(string name)
            : base(TYPE)
        {
            this.Name = name;
        }


        public string Name
        {
            get
            {
                return base["text"];
            }
            set
            {
                base["text"] = value;
                base.FirePropertyChanged("Name");
            }
        }

        public bool IsDone
        {
            get
            {
                return base["completed_at"] != null;
            }
            set
            {
                base["completed_at"] = value ? DateTime.Now.ToString("yyyy-MM-dd") : null;
                base.FirePropertyChanged("IsDone");
            }
        }
    }
}
