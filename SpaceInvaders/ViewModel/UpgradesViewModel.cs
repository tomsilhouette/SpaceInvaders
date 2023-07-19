using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.ViewModel
{
    public partial class UpgradesViewModel : ObservableObject
    {
        public UpgradesViewModel() 
        { 
            
        }

        // BUY LASER
        [RelayCommand]
        private void PurchaseLaser()
        {
            Debug.WriteLine("Im CLICKKKKKED");
        }    
        
        // BUY LIVES
        [RelayCommand]
        private void PurchaseLives()
        {
            Debug.WriteLine("LIFFFFFFFFE");
        }
    }
}
