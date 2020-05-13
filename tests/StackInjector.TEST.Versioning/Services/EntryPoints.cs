using System;
using System.Collections.Generic;
using StackInjector.Attributes;
using StackInjector.Settings;
using StackInjector.Wrappers;

namespace StackInjector.TEST.Versioning.Services
{

    [Service(Version = 1.0)]
    class EntryPointTestMinor : IStackEntryPoint
    {
        [Served(TargetVersion = 1.0)] 
        INiceFilter Answer { get; set; }

        [Served(TargetVersion = 2.0)]
        INiceFilter Nice { get; set; }

        [Served(TargetVersion = 4.0)]
        INiceFilter Bob { get; set; }



        public object EntryPoint ()
        {
            var result = "";

            if( this.Answer.IsNice(42) )
                result += "Answer ";

            if( this.Nice.IsNice(69) )
                result += "Nice ";

            if( this.Bob.IsNice(420) )
                result += "Bob";

            if ( result != "Answer Nice Bob" )
                throw new Exception("Some filter version are wrong! Correct versions: " + result);

            return null;

        }
    }


    class EntryPointTestMajor : IStackEntryPoint
    {
        [Served]
        INiceFilter Bob { get; set; }

        public object EntryPoint()
        {
            if( this.Bob.IsNice(420) )
                return null;

            throw new Exception("Incorrect service version");
        }
    }

}