using System;
using System.Collections.Generic;
using System.Text;

namespace DriversLog
{
   /// <summary>
   /// Provides representation of verb that is used to determine mode file is being opened in. Contained within ProgID.
   /// </summary>
   public class ProgramVerb
   {
      private string command;
      private string name;

      /// <summary>
      /// Gets a value of the command-line path to the program that is to be called when this command verb is used.
      /// </summary>
      public string Command
      {
         get { return command; }
      }

      /// <summary>
      /// Gets the name of the verb representing this command.
      /// </summary>
      /// <example>"open"
      /// "print"</example>
      public string Name
      {
         get { return name; }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="name">Name of verb</param>
      /// <param name="command">Command-line path to program and arguments of associated program</param>
      public ProgramVerb(string name, string command)
      {
         this.name = name;
         this.command = command;
      }
   }
}