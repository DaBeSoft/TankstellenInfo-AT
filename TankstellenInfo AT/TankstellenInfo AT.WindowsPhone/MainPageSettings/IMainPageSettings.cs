using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using SpritpreisrechnerAtClient.Models;

namespace TankstellenInfo_AT.MainPageSettings
{
    public interface IMainPageSettings
    {
        string Title { get;}
        Control PageSpecificControl { get; }
        Task<IEnumerable<SpritInfo>> RefreshAction();

    }
}
