using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;

namespace Golem.Tests.Cael
{
    public class Kentico
    {
        [Test]
        public void AssignPortfolioToAssessor()
        {
            Golem.PageObjects.Cael.Kentico.Login("bkitchener@prototest.com", "Qubit123!")
                .AssignPortfolioToAssessor("", "");
        }
    }
}
