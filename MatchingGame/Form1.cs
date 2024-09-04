using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Fill Board with Icons made
            AssignIconsToSquares();
        }
        // Use this random object to choose random icons for the squares
        Random random = new Random();

        // Each of these letters is an interesting icon in the webdings font, each icon appears twice in the list 
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        // Label References - 

        // firstClicked points to the first Label control that the player clicks, but it will be null if the player hasn't clicked a label yet
        Label firstClicked = null;

        // secondClicked points to the second Label control that the player clicks
        Label secondClicked = null;

        // Assign each icon from the list of icons to a random square
        // The AssignIconsToSquares() method iterates through each label control in the TableLayoutPanel.
        // It runs the same statements for each of them. The statements pull a random icon from the list.
        private void AssignIconsToSquares()
        {
            // The TableLayoutPanel has 16 labels, icon list = 16, icon is pulled at random from list and added to each label 
            // The first line converts the control variable to a label named iconLabel.
            // The second line is an if statement that checks to make sure the conversion worked.
            // If the conversion does work, the statements in the if statement run.
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];

                    // Make icon same as my background to hide
                    iconLabel.ForeColor = iconLabel.BackColor;

                    icons.RemoveAt(randomNumber);
                }
            }
        }

        // Every label's Click event is handled by this event handler
        private void label1_Click(object sender, EventArgs e)
        {
            // The timer is only on after two non-matching icons have been shown to the player
            if (timer1.Enabled == true)
            {
                return;
            }

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed -- ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                {
                    return;
                }

                // If firstClicked is null, this is the first icon in the pair that the player clicked,
                // so set firstClicked to the label that the player clicked, change its color to black, and return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;

                    return;
                }

                // If the player gets this far, the timer isn't running and firstClicked isn't null,
                // so this must be the second icon the player clicked, Set its color to black
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // Check to see if the player won by calling the method 
                WinnerChickenDinner();

                // If the player clicked two matching icons, keep them black and reset firstClicked and secondClicked 
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // If the player gets this far, the player clicked two different icons, so start the 
                // timer (which will wait three quarters of a second, and then hide the icons)
                timer1.Start();
            }
        }

        // It makes sure the timer isn't running by calling the Stop() method. It uses two reference variables,
        // firstClicked and secondClicked, to make the icons of the two labels that the player chose invisible again.
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer 
            timer1.Stop();

            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reset firstClicked and secondClicked (program will know it was the first click)
            firstClicked = null;
            secondClicked = null;
        }

        // Check every icon to see if it is matched by comparing its foreground color the background color 
        // If all icons matched then the player wins and game ends
        private void WinnerChickenDinner()
        {
            // Go through all of the labels in the TableLayoutPanel, checking each one to see if its icon is matched
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconlabel = control as Label;

                if (iconlabel != null)
                {
                    if (iconlabel.ForeColor == iconlabel.BackColor)
                    {
                        return;
                    }    
                }
            }

            // If the loop didn’t return, it didn't find any unmatched icons so all items have been matched
            MessageBox.Show("You matched all the icons! AMAZING!", "Congrats you're a WINNER!");
            Close();
        }
    }
}
