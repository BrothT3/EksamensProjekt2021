﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2021
{
    class Item : GameObject
    {
        public List<Item> items = new List<Item>();

        protected int nextWeapon;

        public int NextWeapon { get => nextWeapon; set => nextWeapon = value; }




        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void OnCollision(GameObject other)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
