using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace KeyVault
{
    public class TopSecretPageViewModel : INotifyPropertyChanged
    {
        IDataService dataService;

        public TopSecretPageViewModel()
        {
            dataService = Xamarin.Forms.DependencyService.Get<IDataService>();
        }

        List<TopSecret> _secrets = new List<TopSecret>();
        public List<TopSecret> Secrets
        {
            get { return _secrets; }
            set
            {
                _secrets = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Secrets)));
            }
        }

        public async Task GetAllSecrets()
        {
            Secrets = await dataService.GetSecretData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
