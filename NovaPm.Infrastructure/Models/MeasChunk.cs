using System;

namespace NovaPm.Infrastructure.Models
{
    public class MeasChunk
    {
        public DateTime TimeDate { get; set; }

        public float Pm2_5Amount { get; set; }

        public float Pm10Amount { get; set; }

        public int Checksum { get; set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return $"{TimeDate.ToShortDateString()}, {TimeDate.ToShortTimeString()}, PM2.5 = {Pm2_5Amount.ToString("G2").PadLeft(6,' ')} ug/m3, PM10 = {Pm10Amount.ToString("G2").PadLeft(6, ' ')} ug/m3, ID = 0x{ID.ToString("X4")}";
        }
    }
}
