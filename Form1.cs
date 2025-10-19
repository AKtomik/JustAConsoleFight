using System;
using Microsoft.VisualBasic.Devices;

namespace JeuDeCombat
{
    public class Settings
    {
    }
    
    public class Character(int maxHp, int atk)
	{
        public static readonly Dictionary<string, Character> all = new()
        {
            {"Damager", new Character(maxHp: 3, atk: 2)},
            {"Healer", new Character(maxHp: 4, atk: 1)},
            {"Tank", new Character(maxHp: 5, atk: 1)},
        };

        //public static string[] = Character.all.Keys.ToArray();
        
        // utilisation des constructeurs primaires
        public int maxHp = maxHp;
        public int atk = atk;
	}


	public class Player
    {
        // character & round
        public string characterName;
        public Character character;
        public int MaxHp
        {
            get => character.maxHp;
        }
        public int Atk
        {
            get => character.atk;
        }
        public double Ratio
        {
            get => hp / MaxHp;
        }

        Round round;
        int roundIndex;
        public Player Enemy
        {
            get => this.round.player[Math.Abs(roundIndex - 1)];
        }
            
        // atributes
        public int hp = 0;

        // compute
        public bool isHuman = false;
        public int botLevel = 0;

        public string lastAction = "";
        public string nextAction = "";
        
        // init
        public Player(Round round, int roundIndex)
		{
            this.round = round;
            this.roundIndex = roundIndex;
		}

        public void SetClass(string characterName)
        {
            this.characterName = characterName;
            this.character = Character.all[characterName];
            hp = character.maxHp;
        }
        
        public void SetHuman()
		{
            isHuman = true;
		}
        public void SetBot(int level)
        {
            isHuman = false;
            botLevel = level;
        }

        // tool
        public bool IsCharacter(string CharacterName)
		{
            return Character.all[CharacterName] == character;
		}

        // play
        public void SetNextAction(string action)
        {
            this.nextAction = action;
        }
        public string GetPlayerTurn()
        {
            if (isHuman)
            {
                this.lastAction = this.nextAction;
                return lastAction;
            }
            else
			{
                this.lastAction = ComputerTurn();
                return lastAction;
			}
        }
        
        string ComputerTurn()
        {
            switch (botLevel)
            {
                case 0:
                    {// do nothing
                        return "AFK";//for compile
                    }
                case 1:
                    {// attack when weak, protect when strong (k that dumb)
                        bool willSpecial = Round.random.Next(1, 4) == 4;
                        if (willSpecial)
                            return "Action Spé";
                        else if (Enemy.hp < this.hp)
                            return "Attaquer";
                        else
                            return "Défendre";
                    }
                case 2:
                    {// random
                        return Form1.AviableActions[Round.random.Next(1, 3)];
                    }
                case 3:
                    {// protect when weak, attack when strong
                        bool willSpecial = Round.random.Next(1, 4) == 4;
                        if (willSpecial)
                            return "Action Spé";
                        else if (Enemy.hp < this.hp)
                            return "Attaquer";
                        else
                            return "Défendre";
                    }
                case 4:
                    {// smarter
                        int probaSpecialPercent = 10;
                        int probaAttackPercent = 50;
                        probaSpecialPercent += (int)Math.Round(this.Ratio * 100);
                        probaSpecialPercent += (int)Math.Round((this.Ratio - Enemy.Ratio) * 100);

                        if (Round.random.Next(100) < probaSpecialPercent)
                            return "Attaque Spé";
                        else if (Round.random.Next(100) < probaAttackPercent)
                            return "Attaquer";
                        else
                            return "Défendre";
                    }
                default:
                    {
                        return "AFK";//for compile
                    }
            }
		}
    }
    
    public class Round
    {

        public int turn = 0;
        public static Random random = new Random();
        public Player[] player;

        public Round()
        {
            player = [new(this, 0), new(this, 1)];
		}

        public void Start()
        {
        }
        
        // take the fight. Return -1 if need to continue. Else return the player index (0/1)
        public int Fight()
        {
            string[] playersActions = [player[0].GetPlayerTurn(), player[1].GetPlayerTurn()];

            turn++;

            bool rageJ = false;
            bool rageO = false;
            //Verification Rage
            if (playersActions[0] == "Action Spé" && player[0].IsCharacter("Tank")) rageJ = true;
            if (playersActions[1] == "Action Spé" && player[1].IsCharacter("Tank")) rageO = true;
            //Joueur
            //Spécial Tank
            if (playersActions[0] == "Action Spé" && player[0].IsCharacter("Tank"))
            {
                player[0].hp--;
                if (playersActions[1] != "Défendre")
                {
                    player[1].hp -= 2;
                    if (rageO)
                    {
                        player[0].hp -= 2;
                    }
                }
                else
                {
                    player[1].hp -= 1;
                }
            }
            //Spécial Healer
            if (playersActions[0] == "Action Spé" && player[0].IsCharacter("Healer"))
            {
                player[0].hp += 2;
            }
            //Attaques
            if (playersActions[0] == "Attaquer")
            {
                if (playersActions[1] != "Défendre")
                {
                    if (player[0].IsCharacter("Damager"))
                    {
                        player[1].hp -= 2;
                    }
                    else
                    {
                        player[1].hp--;
                    }
            
                    if (rageO)
                    {
                        player[0].hp--;
                    }
                }
            }
            
            //ORDINATEUR
            //Spécial Tank
            if (playersActions[0] == "Action Spé" && player[1].IsCharacter("Tank"))
            {
                player[1].hp--;
                if (playersActions[0] != "Défendre")
                {
                    player[0].hp -= 2;
                    if (rageJ)
                    {
                        player[1].hp -= 2;
                    }
                }
                else
                {
                    player[0].hp -= 1;
                }
            }
            //Spécial Healer
            if (playersActions[1] == "Action Spé" && player[1].IsCharacter("Healer"))
            {
                player[1].hp += 2;
            }
            //Attaques
            if (playersActions[1] == "Attaquer")
            {
                if (playersActions[0] != "Défendre")
                {
                    if (player[1].IsCharacter("Damager"))
                    {
                        player[0].hp -= 2;
                    }
                    else
                    {
                        player[0].hp--;
                    }
            
                    if (rageJ)
                    {
                        player[1].hp--;
                    }
                }
            }
            //Empêche d'avoir plus de pv que le max (pour les healers)
            if (player[0].IsCharacter("Healer") && player[0].hp > 4) player[0].hp = 4;
            if (player[1].IsCharacter("Healer") && player[1].hp > 4) player[0].hp = 4;


            if (player[0].hp <= 0) return 0;
            if (player[1].hp <= 0) return 1;
            return -1;
        }
    }

    public partial class Form1 : Form
    {
        //Variable pour les listes de choix
        public static readonly string[] AviableCharacters = ["Damager", "Healer", "Tank" ];
        public static readonly string[] AviableActions = ["Attaquer", "Défendre", "Action Spé" ];
        public static readonly string[] AviableDifficulties = ["1", "2", "3", "4"];
        
        //Variable pour l'UI du menu
        Button ValidButton;
        
        //Variables pour la détection du choix
        string classChoice;
        string actionChoice;
        int difficultyChoice;

        //Variables pour la manche
        Round round;

        public Form1()
        {
            InitializeComponent();
            CreateValidate();
        }

        // UTILITAIRE

        //Créer le bouton valider
        private void CreateValidate()
        {
            ValidButton = new Button();
            Controls.Add(ValidButton);
            ValidButton.Text = "Valider";
            ValidButton.Location = new Point(350, 400);
        }
        
        private EventHandler _currentHandler;
        private void SetValidate(EventHandler newEvent)
        {
            if (_currentHandler != null) ValidButton.Click -= _currentHandler;
            _currentHandler = newEvent;
            ValidButton.Click += newEvent;
        }
        private void SetText(string message)
        {
            textClass.Text = message;
        }
        
        //Faire apparaitre les différents boutons de choix
        private void PutButton(string[] ListOfText, EventHandler actionToDo)
        {
            for (int i = 0; i < ListOfText.Length; i++)
            {
                RadioButton buttonClasse = new RadioButton();
                choiceButtons.Controls.Add(buttonClasse);
                buttonClasse.Location = new Point(25 + 150 * i, 25);
                buttonClasse.Text = ListOfText[i];
                buttonClasse.Click += new EventHandler(actionToDo);
            }
        }


        // JOUER LES PARAMÈTRES
        // les fonctions forment une chaine et sont dans l'odre

        // étape de sélection de la classe
        private void Form1_Load(object? sender, EventArgs e)
        {
            SetText("Veuillez choisir une classe.");
            PutButton(AviableCharacters, characterChoose);
            SetValidate(ChooseDifficulty);
            ValidButton.Hide();
        }

        // étape de sélection de la difficulté
        private void ChooseDifficulty(object? sender, EventArgs e)
        {
            choiceButtons.Controls.Clear();
            choiceButtons.Text = "Difficulté";
            textClass.Text = "Choisissez une difficulté";
            PutButton(AviableDifficulties, difficultyToChoose);
            SetValidate(PlayRound);
            ValidButton.Hide();
            
        }
        //Permet de sélectionner une difficulté via les boutons
        private void difficultyToChoose(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi la difficulté " + buttonTook;
            difficultyChoice = int.Parse(buttonTook);
            ValidButton.Show();
        }
        //Permet de sélectionner une classe via les boutons
        private void characterChoose(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi " + buttonTook + ".";
            classChoice = buttonTook;
            ValidButton.Show();
        }

        // JOUER LA MANCHE
        // les fonctions Play() sont des boucles

        // lancer une manche
        private void PlayRound(object? sender, EventArgs e)
        {
            // round init
            round = new Round();

            // human player set
            round.player[0].SetHuman();
            round.player[0].SetClass(classChoice);

            // bot player set
            int ClassInt = Round.random.Next(Character.all.Keys.Count) + 1;
            string classeChoisieBot = Character.all.Keys.ToArray()[ClassInt - 1];
            round.player[1].SetBot(difficultyChoice);
            round.player[1].SetClass(classeChoisieBot);

            // ui
            TurnUiUpdate();
            SetValidate(PlayTurn);
            ValidButton.Hide();
		}

        // prohcain tour
        private void PlayTurn(object? sender, EventArgs e)
        {
            int winner = round.Fight();
            if (winner != -1)
            {
                VictoryUi(winner);
                return;
            }

            // ui
            TurnUiUpdate();
            name.Text = "winner:" + winner;

            ValidButton.Show();
        }
        
        // monter l'interface du choix d'action
        private void TurnUiUpdate()
		{
            name.Text = "tour " + (round.turn + 1);
            choiceButtons.Hide();
            choiceButtons.Controls.Clear();
            choiceButtons.Text = "Action";
            textClass.Hide();
            textClass.Text = "Choisissez une action.";
            PutButton(AviableActions, actionChoose);
            choiceButtons.Show();
            textClass.Show();
            ValidButton.Hide();
            
            Stats(playerStat, round.player[0].hp, round.player[0].characterName, round.player[0].lastAction);
            Stats(ordiStats, round.player[1].hp, round.player[1].characterName, round.player[1].lastAction);
		}

        //Permet la création et le choix des boutons d'attaques
        private void actionChoose(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            actionChoice = classr.Text;
            textClass.Text = "Vous avez choisi " + actionChoice + ".";
            round.player[0].SetNextAction(actionChoice);
            ValidButton.Show();
        }
        
        //Gère la création et la mise à jour des stats
        private void Stats(GroupBox StatsBox, int pv, string classPlayer, string lastAction)
        {
            StatsBox.Controls.Clear();
            List<string> statsList = new List<string> { "PV : ", "Classe : ", "Dernière action : ", lastAction };
            List<string> statsString = new List<string>() { pv.ToString(), classPlayer, "", "" };
            
            for (int i = 0; i < statsList.Count; i++)
            {
                Label statsText= new Label();
                StatsBox.Controls.Add(statsText);
                statsText.Location = new Point(5, 25*i+25);
                statsText.Text = statsList[i] + statsString[i];
            }
            StatsBox.Show();
        }

        //Gère la fin du match
        private void VictoryUi(int winner)
        {
            name.Hide();
            choiceButtons.Hide();
            playerStat.Hide();
            ordiStats.Hide();
            textClass.Hide();
            ValidButton.Click += new EventHandler (endForm);
            if (winner == -2)
            {
                name.Text = "Vous avez fait égalité.";
            }
            else if (winner == 1)
            {
                name.Text = "Vous avez perdu.";
            }
            else if (winner == 0)
            {
                name.Text = "Vous avez gagné.";
            }
            name.TextAlign = ContentAlignment.TopCenter;
            name.Show();
        }

        private void endForm(object? sender, EventArgs e)
        {
            Close();
        }

    }
}

