using System;
using System.IO;

namespace TheMazeRunner
{
	public enum ParserState
	{
		PacketSize,
		PacketBody
	}

	public class PacketParser
	{
		private readonly CircularBuffer buffer;
		private int packetSize;
		private ParserState state;
		private readonly byte[] cache = new byte[8];
		public const int OuterPacketSizeLength = 2;
		public MemoryStream MemoryStream;

		public PacketParser(CircularBuffer _buffer)
		{
			this.buffer = _buffer;
		}

		public bool Parse()
		{
			while (true)
			{
				switch (this.state)
				{
					case ParserState.PacketSize:
					{
						if (this.buffer.Length < OuterPacketSizeLength)
						{
							return false;
						}

						this.buffer.Read(this.cache, 0, OuterPacketSizeLength);

						this.packetSize = BitConverter.ToUInt16(this.cache, 0);
						if (this.packetSize < OuterPacketSizeLength)
						{
							throw new Exception($"recv packet size error, 可能是外网探测端口: {this.packetSize}");
						}
						this.state = ParserState.PacketBody;
						break;
					}
					case ParserState.PacketBody:
					{
						if (this.buffer.Length < this.packetSize)
						{
							return false;
						}

						MemoryStream _memoryStream = new MemoryStream(this.packetSize);
						this.buffer.Read(_memoryStream, this.packetSize);
						//memoryStream.SetLength(this.packetSize - Packet.MessageIndex);
						this.MemoryStream = _memoryStream;
						this.state = ParserState.PacketSize;
						return true;
					}
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}