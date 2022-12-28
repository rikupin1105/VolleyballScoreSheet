using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDialogLib
{
    public interface ICommonDialogSettings
    {
        string InitialDirectory { get; set; }

        string Title { get; set; }
    }
}
