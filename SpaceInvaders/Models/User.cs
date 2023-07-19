using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Models
{
    public class User
    {
        public string Username { get; set; }
        public int Score { get; set; }

        public User(string username, int score)
        {
            Username = username;
            Score = score;
        }
    }
}
