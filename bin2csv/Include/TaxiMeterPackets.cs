using System;
using System.Text;
using TaxiMeter.Utils;

namespace TaxiMeter.Packets
{

	static class PacketConstants: System.Object {
		public const byte DLE = 0x10;
		public const byte ETX = 0x03;
		public const byte PacketTypeB2 = 0xB2;
		public const byte PacketTypeB4 = 0xB4;
		public const byte STX = 0x02;
	}

	abstract class Packet: System.Object {
		private DateTime date;
		private char driverType;
		private string licensePlateNumber;

		public abstract string AsString();

		public virtual void Decode(byte[] data) {
			date = Utils.Helpers.EpochToDateTime(Utils.Helpers.GetUInt32(data, 0));
			char[] number = Utils.Helpers.GetString(data, 4, 7);
			StringBuilder sBuild = new StringBuilder ("");
			licensePlateNumber = sBuild.Append(number).ToString().Replace("\0", "");
			driverType = (char)data [11];
		}

        public string SingleToString(Single value)
        {
           double temp = value;
           return temp.ToString("F4");
        }

		public abstract string GetPacketType();

		public Boolean IsValid() {
			return (licensePlateNumber.Length > 0);
		}

		public DateTime Date {
			get {
				return date;
			}
			set {
				date = value;
			}
		}

		public char DriverType {
			get {
				return driverType;
			}
			set {
				driverType = value;
			}
		}

		public string LicensePlateNumber {
			get {
				return licensePlateNumber;
			}
			set {
				licensePlateNumber = value;
			}
		}
	}

	class PacketB4: Packet {
		private UInt16 fare;
		private float getOffLat;
		private float getOffLng;
		private DateTime getOffTime;
		private UInt16 operatingDuration;
		private UInt32 operatingMileage;
		private float pickupLat;
		private float pickupLng;
		private DateTime pickupTime;
		private UInt16 stopTime;
		private UInt16 toll;

		public override string AsString() {
            double temp;
			StringBuilder sb = new System.Text.StringBuilder ();
			sb.Append(Date.ToString("yyyy/MM/dd"));
			sb.Append(",");
			sb.Append(LicensePlateNumber);
			sb.Append(",");
			sb.Append(DriverType);
			sb.Append(",");
			sb.Append(pickupTime.ToString("yyyy/MM/dd HH:mm:ss"));
			sb.Append(",");
			sb.Append(getOffTime.ToString("yyyy/MM/dd HH:mm:ss"));
			sb.Append(",");
			sb.Append(operatingMileage.ToString());
			sb.Append(",");
			sb.Append(operatingDuration.ToString());
			sb.Append(",");
			sb.Append(stopTime.ToString());
			sb.Append(",");
			sb.Append(fare.ToString());
			sb.Append(",");
			sb.Append(toll.ToString());
			sb.Append(",");
            sb.Append(SingleToString(pickupLng));
			sb.Append(",");
            sb.Append(SingleToString(pickupLat));
			sb.Append(",");
            sb.Append(SingleToString(getOffLng));
			sb.Append(",");
            sb.Append(SingleToString(getOffLat));
			return sb.ToString();
		}

		public override void Decode(byte[] data) {
			base.Decode(data);
			pickupTime = Utils.Helpers.EpochToDateTime(Utils.Helpers.GetUInt32(data, 12));
			getOffTime = Utils.Helpers.EpochToDateTime(Utils.Helpers.GetUInt32(data, 16));
			operatingMileage = Utils.Helpers.GetUInt32(data, 20);
			operatingDuration = Utils.Helpers.GetUInt16(data, 24);
			stopTime = Utils.Helpers.GetUInt16(data, 26);
			fare = Utils.Helpers.GetUInt16(data, 28);
			toll = Utils.Helpers.GetUInt16(data, 30);
			pickupLng = Utils.Helpers.GetSingle(data, 32);
			pickupLat = Utils.Helpers.GetSingle(data, 36);
			getOffLng = Utils.Helpers.GetSingle(data, 40);
			getOffLat = Utils.Helpers.GetSingle(data, 44);
		}

		public override string GetPacketType() {
			return "B4";
		}

		public UInt16 Fare {
			get {
				return fare;
			}
			set {
				fare = value;
			}
		}

		public float GetOffLat {
			get {
				return getOffLat;
			}
			set {
				getOffLat = value;
			}
		}

		public float GetOffLng {
			get {
				return getOffLng;
			}
			set {
				getOffLng = value;
			}
		}

		public DateTime GetOffTime {
			get {
				return getOffTime;
			}
			set {
				getOffTime = value;
			}
		}

		public UInt16 OperatingDuration {
			get {
				return operatingDuration;
			}
			set {
				operatingDuration = value;
			}
		}

		public UInt32 OperatingMileage {
			get {
				return operatingMileage;
			}
			set {
				operatingMileage = value;
			}
		}

		public float PickupLat {
			get {
				return pickupLat;
			}
			set {
				pickupLat = value;
			}
		}

		public float PickupLng {
			get {
				return pickupLng;
			}
			set {
				pickupLng = value;
			}
		}

		public DateTime PickupTime {
			get {
				return pickupTime;
			}
			set {
				pickupTime = value;
			}
		}

		public UInt16 StopTime {
			get {
				return stopTime;
			}
			set {
				stopTime = value;
			}
		}

		public UInt16 Toll {
			get {
				return toll;
			}
			set {
				toll = value;
			}
		}
	}

	class PacketB2: Packet {
		private UInt32 operatingDuration;
		private UInt32 operatingMileage;

		public override string AsString() {
			StringBuilder sb = new System.Text.StringBuilder ();
			sb.Append(Date.ToString("yyyy/MM/dd"));
			sb.Append(",");
			sb.Append(LicensePlateNumber);
			sb.Append(",");
			sb.Append(DriverType);
			sb.Append(",");
			sb.Append(operatingDuration.ToString());
			sb.Append(",");
			sb.Append(operatingMileage.ToString());
			return sb.ToString();
		}

		public override void Decode(byte[] data) {
			base.Decode(data);
			operatingDuration = Utils.Helpers.GetUInt32(data, 12);
			operatingMileage = Utils.Helpers.GetUInt32(data, 16);
		}

		public override string GetPacketType() {
			return "B2";
		}

		public UInt32 OperatingDuration {
			get {
				return operatingDuration;
			}
			set {
				operatingDuration = value;
			}
		}

		public UInt32 OperatingMileage {
			get {
				return operatingMileage;
			}
			set {
				operatingMileage = value;
			}
		}
	}
}
