
using System.Globalization;
namespace ZBRA.Impress.Globalization
{
    public interface IMessageProcessor
    {

        MessageTranslation Translate(CultureInfo culture, string key, object[] parameters);
    }
}
