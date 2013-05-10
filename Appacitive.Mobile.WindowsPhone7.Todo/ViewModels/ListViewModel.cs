using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Appacitive.Mobile.WindowsPhone7.Todo.ViewModels
{
    public class ListViewModel : Sdk.Article
    {
        private const string TYPE = "todolists";

        public ListViewModel(Sdk.Article article)
            : base(article)
        {

        }
        public ListViewModel(string name)
            : base(TYPE)
        {
            this.Name = name;
        }


        public const string ARTICLE_TYPE = "TodoLists";

        public string Name
        {
            get
            {
                return base["list_name"];
            }
            set
            {
                base["list_name"] = value;
                base.FirePropertyChanged("Name");
            }
        }
    }
}
