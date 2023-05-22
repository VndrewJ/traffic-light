using System;
using System.Collections.Generic;
using System.Text;

namespace MECHENG_313_A2.Tasks
{
    internal class Task3 : Task2
    {
        public override TaskNumber TaskNumber => TaskNumber.Task3;

        private int redLength = 1000;
        private int greenLength = 1000;
        private int defaultLength = 1000; 

        // TODO: Implement this

        public override void ConfigLightLength(int redLength, int greenLength)
        {
            //No need for task 2. 
            this.redLength = redLength;
            this.greenLength = greenLength; 
        }
    }
}
