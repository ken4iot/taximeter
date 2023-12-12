using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

public class SERIAL2BIN
{
	static bool _continue;
	static SerialPort _serialPort;
	static FileStream fileStream;
	public static void Main(string[] args)
	{
		int baudRate;
		if (args.Length!=1) {
			Console.WriteLine("Ex: ser2bin 115200/57600");
			return;
		}
		else
		{
			try {
				baudRate=Convert.ToInt32(args[0]);
				if (baudRate!=115200 && baudRate!=57600) {
					Console.WriteLine("Baud rate should be 115200 or 57600.");
					return;
				}
			}
			catch (Exception) {
				Console.WriteLine("Baud rate should be 115200 or 57600.");
				return;
			}
		}
		Thread readThread = new Thread(Read);
		string strBinFile = string.Format("{0}.bin",DateTime.Now.ToString("yyyyMMddHHmmss"));

		// Create a new SerialPort object with default settings.

		_serialPort = new SerialPort();
		_serialPort.PortName = SetPortName(_serialPort.PortName);   
		_serialPort.BaudRate = baudRate;
		_serialPort.Parity = Parity.None;
		_serialPort.DataBits = 8;
		_serialPort.StopBits = StopBits.One; 

		// Set the read/write timeouts
		_serialPort.ReadTimeout = 500;
		_serialPort.WriteTimeout = 500;

		try	{
			_serialPort.Open();
		}
		catch(Exception e)
		{
			Console.WriteLine("Can't open COM port because of "+ e.GetType().Name);
			return;
		}
		fileStream=new FileStream(strBinFile,FileMode.Create);
		Console.WriteLine("Ready for reading "+_serialPort.PortName);
		Console.WriteLine("[ESC] to quit...");
		_continue = true;    
		readThread.Start();
		while (_continue)
		{

			if (Console.ReadKey().Key==ConsoleKey.Escape)
			{
				Console.WriteLine("\n File: "+strBinFile+" created!");
				_continue = false;
			}
		}
		readThread.Join();
		fileStream.Close();
		_serialPort.Close();
	}

	public static void Read()
	{
		int i=0;
		while (_continue)
		{
			try
			{
				fileStream.WriteByte((byte)_serialPort.ReadByte());
				if (++i%100==0)
					Console.Write('#');
			}
			catch (TimeoutException) { }
		}
	}
    // Display Port values and prompt user to enter a port.
    public static string SetPortName(string defaultPortName)
    {
        string portName;

        Console.WriteLine("Available Ports:");
        foreach (string s in SerialPort.GetPortNames())
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
        portName = Console.ReadLine();

        if (portName == "" || !(portName.ToLower()).StartsWith("com"))
        {
            portName = defaultPortName;
        }
        return portName;
    }
}