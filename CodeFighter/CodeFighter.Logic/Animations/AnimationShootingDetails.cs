using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Animations
{
    public class AnimationShootingDetails : IAnimationDetails
    {
        public List<AnimationShotDetails> shots { get; set; }

        public AnimationShootingDetails(List<AnimationShotDetails> shots)
        {
            this.shots = shots;
        }
    }
}
