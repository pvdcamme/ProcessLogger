// See https://aka.ms/new-console-template for more information
using System.Diagnostics;


var currentProcess = Process.GetProcesses();
Console.WriteLine("Currently running " + currentProcess.Length + " processes");