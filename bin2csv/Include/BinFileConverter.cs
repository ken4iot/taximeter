using System;
using System.IO;
using System.Collections.Generic;
using TaxiMeter.Packets;

namespace TaxiMeter.Converter
{
	public class BinFileConverter: System.Object {
		public BinFileConverter() {
		}
		Dictionary<string, StreamWriter> csvFiles = new Dictionary<string, StreamWriter> ();

		private Packet CreatePacket(int packetType) {
			Packet result;
			switch (packetType) {
			case PacketConstants.PacketTypeB2:
				result = new PacketB2 ();
				break;

			case PacketConstants.PacketTypeB4:
				result = new PacketB4 ();
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		private void writeToCSV(Packet packet) {
			string licensePlateNumber = packet.LicensePlateNumber;
			string packetType = packet.GetPacketType();
			string fileName = "[" + packetType + "]" + licensePlateNumber + ".csv";
			StreamWriter writer = null;
			if (!csvFiles.ContainsKey(fileName)) {
				writer = new StreamWriter (fileName);
				csvFiles.Add(fileName, writer);
				if (packetType == "B2") {
					writer.WriteLine("Date,Plate,Driver,Total_Time,Total_Dist");
				} else if (packetType == "B4") {
					writer.WriteLine("Date,Plate,Driver,Start,End,Dist,Time,Wait,Total,Toll,Start_Lon,Start_Lat,End_Lon,End_Lat");
				}
			} else {
				csvFiles.TryGetValue(fileName, out writer);
			}
			if (writer != null) {
				writer.WriteLine(packet.AsString());
				writer.Flush();
			}
		}

		public void Convert(string fileName) {
			FileStream binFile = new FileStream (fileName, FileMode.Open);
			binFile.Position = 0;
			int BinFileLen = (int)binFile.Length;
			BinaryReader reader = new BinaryReader (binFile);

			while (binFile.Position < BinFileLen) {
				int b = reader.ReadByte();
				if (b == PacketConstants.STX) {
					int command = reader.ReadByte();
					Packet packet = CreatePacket(command);
					if (packet != null) {
						UInt32 length = TaxiMeter.Utils.Helpers.ReadUInt32BE(reader);
						if ((length + binFile.Position) <= BinFileLen) {
							byte[] buffer = reader.ReadBytes((int)length);
							packet.Decode(buffer);
							int b1 = reader.ReadByte();
							int b2 = reader.ReadByte();
							if ((b1 == PacketConstants.ETX) && (b2 == PacketConstants.DLE)) {
								if (packet.IsValid()) {
									writeToCSV(packet);
								}
							}
						}
					}
				}
			}

			foreach (var item in csvFiles) {
				StreamWriter writer = item.Value;
				writer.Close();
			}

			csvFiles.Clear();
		}
	}
}

