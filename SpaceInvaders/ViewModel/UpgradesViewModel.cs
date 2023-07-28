using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace SpaceInvaders.ViewModel
{
    public partial class UpgradesViewModel : ObservableObject
    {
        public GameState State { get; set; }
        public GameViewModel GameViewModel { get; set; }

        public UpgradesViewModel(GameState state, GameViewModel gameViewModel) 
        { 
            State = state;
            GameViewModel = gameViewModel;
        }

        // BUY LASER
        [RelayCommand]
        private void PurchaseLaser()
        {
            if (State.FinishingScore >= State.LaserCost)
            {
                Debug.WriteLine("LAZZZZZZZZZZZER");
                State.PlayerLasersOwned++;
                State.FinishingScore -= State.LaserCost;           
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
            // If not greater than max lives and has the cash
            if (State.FinishingScore >= State.ExtraLifeCost && State.PlayerLives < State.MaxPlayerLives)
            {
                if (State.PlayerLives == 0)
                {
                    GameViewModel.CanViewOne = true;
                    GameViewModel.CanViewTwo = false;
                    GameViewModel.CanViewThree = false;
                }
                else if (State.PlayerLives == 1)
                {
                    GameViewModel.CanViewOne = true;
                    GameViewModel.CanViewTwo = true;
                    GameViewModel.CanViewThree = false;
                }
                else if (State.PlayerLives == 2)
                {
                    GameViewModel.CanViewOne = true;
                    GameViewModel.CanViewTwo = true;
                    GameViewModel.CanViewThree = true;
                }
                State.PlayerLives++;
                State.FinishingScore -= State.ExtraLifeCost;
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
                State.FinishingScore -= State.EnergyShieldCost;
            } 
            else
            {
                throw new Exception();
            }
        }
    }
}
