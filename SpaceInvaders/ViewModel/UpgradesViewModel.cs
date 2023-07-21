using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace SpaceInvaders.ViewModel
{
    public partial class UpgradesViewModel : ObservableObject
    {
        public GameState State { get; set; }

        public UpgradesViewModel(GameState state) 
        { 
            State = state;
        }

        // BUY LASER
        [RelayCommand]
        private void PurchaseLaser()
        {
            if (State.FinishingScore >= State.LaserCost)
            {
                Debug.WriteLine("LAZZZZZZZZZZZER");
                State.PlayerLasersOwned++;
            }
            else
            {
                throw new Exception();
            }
        }    
        
        // BUY LIVES
        [RelayCommand]
        private void PurchaseLives()
        {
            if (State.FinishingScore >= State.ExtraLifeCost && State.PlayerLasersOwned < State.MaxPlayerLives)
            {
                Debug.WriteLine("LIFFFFFFFFE");
                State.PlayerLives++;
            }
            else
            {
                throw new Exception();
            }
        }        
        
        // BUY SHIELDS
        [RelayCommand]
        private void PurchaseShield()
        {
            if (State.FinishingScore >= State.EnergyShieldCost)
            {
                Debug.WriteLine("SHIELDDDDD");
                State.PlayerShieldsOwned++;
            } 
            else
            {
                throw new Exception();
            }
        }
    }
}
