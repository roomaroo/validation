namespace DataModel.Dto
{
    [AutoValidate.Validate]
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}