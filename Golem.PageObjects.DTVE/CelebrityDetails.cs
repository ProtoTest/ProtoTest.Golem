using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.WebDriver.Elements.Types;
using OpenQA.Selenium;

namespace Golem.PageObjects.DTVE
{
    public class CelebrityDetails
    {
        public string id; 
        public CelebrityDetails(string id)
        {
            this.id = id;
        }


    }
}

