using System;
using System.Collections.Generic;
using Base;

namespace DartApp.Models
{
    public class Player : ModelBase
    {
	    private string vorName = "";
	    private string nachName = "";
        #region ctors
        public Player()
            :this("", "", DateTime.Today, "")
        {
        }

        public Player(string vname, string nname, DateTime geb, string imageName)
			:base()
        {
	        this.VorName = vname;
	        this.NachName = nname;
	        this.Geb = geb;
	        this.ImageName = imageName;
        }

        public Player(List<string> itemArray)
        {
            this.Id = itemArray[0];
	        this.VorName = itemArray[1];
	        this.NachName = itemArray[2];
	        this.Geb = DateTime.Parse(itemArray[3]);
	        this.ImageName = itemArray[4];
        }
        #endregion

        #region properties
        public string VorName
		{
	        get
	        {
		        return this.vorName;
	        }
	        set
	        {
		        this.vorName = value;
		        SetDisplayName();
	        }
		}
        public string NachName
		{
			get
			{
				return this.nachName;
			}
			set
			{
				this.nachName = value;
				SetDisplayName();
			}
		}
		public DateTime Geb { get; set; }
        public string ImageName { get; set; }
        #endregion

	    private void SetDisplayName()
	    {
		    this.DisplayName = this.NachName + " " + this.VorName;
	    }

		public static Player Copy(Player targetPlayer)
		{
			Player copy = new Player();
			copy.Id = targetPlayer.GetId();
			copy.VorName = targetPlayer.VorName;
			copy.NachName = targetPlayer.NachName;
			copy.Geb = targetPlayer.Geb;
			copy.ImageName = targetPlayer.ImageName;
			return copy;
		}
    }
}
