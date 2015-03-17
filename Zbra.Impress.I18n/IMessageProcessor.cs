
using System.Globalization;
namespace Zbra.Impress.Globalization
{
    public interface IMessageProcessor
    {

        MessageTranslation Translate(CultureInfo culture, string key, object[] parameters);
    }
}
