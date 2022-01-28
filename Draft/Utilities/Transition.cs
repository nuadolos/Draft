using Draft.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Draft.Utilities
{
    internal class Transition
    {
        public static Frame MainFrame { get; set; }

        private static DraftEntities context;
        public static DraftEntities Context 
        { 
            get
            {
                if (context == null)
                    context = new DraftEntities();

                return context;
            } 
        }
    }
}
