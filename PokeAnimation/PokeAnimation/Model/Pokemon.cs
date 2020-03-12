namespace PokeAnimation.Model
{
    public class Ability
    {
        #region Properties

        public Ability1 ability { get; set; }
        public bool is_hidden { get; set; }
        public int slot { get; set; }

        #endregion Properties
    }

    public class Ability1
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }

    public class Form
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }

    public class Game_Indices
    {
        #region Properties

        public int game_index { get; set; }
        public Version version { get; set; }

        #endregion Properties
    }

    public class Move
    {
        #region Properties

        public Move1 move { get; set; }
        public Version_Group_Details[] version_group_details { get; set; }

        #endregion Properties
    }

    public class Move_Learn_Method
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }

    public class Move1
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }

    public class Pokemon
    {
        #region Properties

        public Ability[] abilities { get; set; }
        public int base_experience { get; set; }
        public Form[] forms { get; set; }
        public Game_Indices[] game_indices { get; set; }
        public int height { get; set; }
        public object[] held_items { get; set; }
        public int id { get; set; }
        public bool is_default { get; set; }
        public string location_area_encounters { get; set; }
        public Move[] moves { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public Species species { get; set; }
        public Sprites sprites { get; set; }
        public Stat[] stats { get; set; }

        //public Type[] types { get; set; }
        public int weight { get; set; }

        #endregion Properties
    }

    public class Species
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }

    public class Sprites
    {
        #region Properties

        public string back_default { get; set; }
        public object back_female { get; set; }
        public string back_shiny { get; set; }
        public object back_shiny_female { get; set; }
        public string front_default { get; set; }
        public object front_female { get; set; }
        public string front_shiny { get; set; }
        public object front_shiny_female { get; set; }

        #endregion Properties
    }

    public class Stat
    {
        #region Properties

        public int base_stat { get; set; }
        public int effort { get; set; }
        public Stat1 stat { get; set; }

        #endregion Properties
    }

    public class Stat1
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }

    //public class Type
    //{
    //    #region Properties

    //    public int slot { get; set; }
    //    public Type1 type { get; set; }

    //    #endregion Properties
    //}

    public class Type1
    {
        #region Properties

        public string name { get; set; }
        public string TipoColor { get; set; }
        public string url { get; set; }

        #endregion Properties
    }

    public class Version
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }

    public class Version_Group
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }

    public class Version_Group_Details
    {
        #region Properties

        public int level_learned_at { get; set; }
        public Move_Learn_Method move_learn_method { get; set; }
        public Version_Group version_group { get; set; }

        #endregion Properties
    }
}