using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.ViewModel
{
    public class HighScoresViewModel : ObservableObject
    {
        public List<HighScoreRecord> HighScoresList { get; set; } = new List<HighScoreRecord>();
        public record HighScoreRecord(string Name, int Score);

        public HighScoresViewModel() {
            HighScoresList.Add(new HighScoreRecord(Name: "Tom", Score: 111));
            HighScoresList.Add(new HighScoreRecord(Name: "Jill", Score: 53));
            HighScoresList.Add(new HighScoreRecord(Name: "Dave", Score: 234));

            // HighScoresList.OrderBy(r => r.Score).ToList();
            HighScoresList.Sort((x, y) => y.Score.CompareTo(x.Score)); 

        }

    }
}
