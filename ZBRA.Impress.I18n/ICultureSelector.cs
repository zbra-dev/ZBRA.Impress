using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Globalization
{
    public interface ICultureSelector
    {
        CultureInfo SelectCulture(CulturePreference[] preferences);
    }
}
