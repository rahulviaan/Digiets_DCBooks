using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace DC
{
    public enum eBookStatus
    {
        [Description("Add To Cart")]
        AddToCart = 0,
        [Description("Read Book")]
        InCart = 1,
        [Description("In Cart")]
        ReadBook,
        

    }

    
    public enum LoginSourse
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("Google")]
        Google = 0,
        [Description("Twitter")]
        Twitter,
        [Description("Facebook")]
        Facebook,
        [Description("DC")]
        DC,
        
    }
    public enum Gender
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("Male")]
        Male = 0,
        [Description("Female")]
        Female 
    }
    public enum LoginMode
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("Android")]
        Android = 0,
        [Description("Iphone")]
        Iphone,
        [Description("Windows")]
        Windows,
        [Description("Browser")]
        Browser,
        [Description("Others")]
        Others,
    }
    public enum BookType
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("Fixed Epub")]
        Fixed_Epub = 0,
        [Description("Reflow Epub")]
        Reflow_Epub,
        [Description("PDF Book")]
        PDF_Book
    }
    public enum TimeRange
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("12.00 AM")]
        zero = 0,
        [Description("01.00 AM")]
        one,
        [Description("02.00 AM")]
        two,
        [Description("03.00 AM")]
        three,
        [Description("04.00 AM")]
        four,
        [Description("05.00 AM")]
        five,
        [Description("06.00 AM")]
        six,
        [Description("07.00 AM")]
        seven,
        [Description("08.00 AM")]
        eight,
        [Description("09.00 AM")]
        nine,
        [Description("10.00 AM")]
        ten,
        [Description("11.00 AM")]
        eleven,
        [Description("12.00 PM")]
        twelve,
        [Description("01.00 PM")]
        thirteen,
        [Description("02.00 PM")]
        fourteen,
        [Description("03.00 PM")]
        fifteen,
        [Description("04.00 PM")]
        sixteen,
        [Description("05.00 PM")]
        seventeen,
        [Description("06.00 PM")]
        eightenn,
        [Description("07.00 PM")]
        nineteen,
        [Description("08.00 PM")]
        twenty,
        [Description("09.00 PM")]
        twentyone,
        [Description("10.00 PM")]
        twentytwo,
        [Description("11.00 PM")]
        twentythree,
    }
    public enum Minutes
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("00 Min")]
        zero,
        [Description("01 Min")]
        one,
        [Description("02 Min")]
        two,
        [Description("03 Min")]
        three,
        [Description("04 Min")]
        four,
        [Description("05 Min")]
        five,
        [Description("06 Min")]
        six,
        [Description("07 Min")]
        seven,
        [Description("08 Min")]
        eight,
        [Description("09 Min")]
        nine,
        [Description("10 Min")]
        ten,
        [Description("11 Min")]
        eleven,
        [Description("12 Min")]
        twelve,
        [Description("13 Min")]
        thirteen,
        [Description("14 Min")]
        fourteen,
        [Description("15 Min")]
        fifteen,
        [Description("16 Min")]
        sixteen,
        [Description("17 Min")]
        seventeen,
        [Description("18 Min")]
        eighteen,
        [Description("19 Min")]
        nineteen,
        [Description("20 Min")]
        twenty,
        [Description("21 Min")]
        twentyone,
        [Description("22 Min")]
        twentytwo,
        [Description("23 Min")]
        twentythree,
        [Description("24 Min")]
        twentyfour,
        [Description("25 Min")]
        twentyfive,
        [Description("26 Min")]
        twentysix,
        [Description("27 Min")]
        twentyseven,
        [Description("28 Min")]
        twentyeight,
        [Description("29 Min")]
        twentynine,

        [Description("30 Min")]
        thirty,
        [Description("31 Min")]
        thirtyone,
        [Description("32 Min")]
        thirtytwo,
        [Description("33 Min")]
        thirtythree,
        [Description("34 Min")]
        thirtyfour,
        [Description("35 Min")]
        thirtyfive,
        [Description("36 Min")]
        thirtysix,
        [Description("37 Min")]
        thirtyseven,
        [Description("38 Min")]
        thirtyeight,
        [Description("39 Min")]
        thirtynine,
        [Description("40 Min")]
        forty,
        [Description("41 Min")]
        fortyone,
        [Description("42 Min")]
        fortytwo,
        [Description("43 Min")]
        fortythree,
        [Description("44 Min")]
        fortyfour,
        [Description("45 Min")]
        fortyfive,
        [Description("46 Min")]
        fortysix,
        [Description("47 Min")]
        fortyseven,
        [Description("48 Min")]
        fortyeight,
        [Description("49 Min")]
        fortynine,
        [Description("50 Min")]
        fifty,
        [Description("51 Min")]
        fiftyone,
        [Description("52 Min")]
        fiftytwo,
        [Description("53 Min")]
        fiftythree,
        [Description("54 Min")]
        fiftyfour,
        [Description("55 Min")]
        fiftyfive,
        [Description("56 Min")]
        fiftysix,
        [Description("57 Min")]
        fiftyseven,
        [Description("58 Min")]
        fiftyeight,
        [Description("59 Min")]
        fiftynine,

    }
    public enum WeekDays
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("Monday")]
        Monday,
        [Description("Tuesday")]
        Tuesday,
        [Description("Wednesday")]
        Wednesday,
        [Description("Thursday")]
        Thursday,
        [Description("Friday")]
        Friday,
        [Description("Saturday")]
        Saturday,
        [Description("Sunday")]
        Sunday,
    }



    public enum Action
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("Insert")]
        Insert = 0,
        [Description("Update")]
        Update,
        [Description("Delete")]
        Delete,

    }

    public enum PaymentStatus
    {
        Initiated=1,
        Successful=2,
        Failed=3,
        Aborted=4,
        Invalid=5

    }
    public enum Sourse
    {
        [Description("N/A")]

        All = -1,
        [Description("SelfWebsite")]
        SelfWebsite = 0,
        [Description("Google")]
        Google,
        [Description("Twitter")]
        Twitter,
        [Description("Facebook")]
        Facebook,
        [Description("DC")]
        DC,
    }

    public enum Status
    {
        [Description("All")]
        [EnumDetail(LongDescription = "All", ShortDescription = "All", Code = 0)]
        All = -1,
        [Description("In-Active")]
        [EnumDetail(LongDescription = "In-Active", ShortDescription = "In-Active", Code = 1)]
        InActive = 0,
        [Description("Active")]
        [EnumDetail(LongDescription = "Active", ShortDescription = "Active", Code = 2)]
        Active,
    }

    public enum enWeekDays
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("Monday")]
        Monday,
        [Description("Tuesday")]
        Tuesday,
        [Description("Wednesday")]
        Wednesday,
        [Description("Thursday")]
        Thursday,
        [Description("Friday")]
        Friday,
        [Description("Saturday")]
        Saturday,
        [Description("Sunday")]
        Sunday,
    }
    public enum AddressType
    {
        [Description("Unspecified")]
        NA = -1,
        [Description("Delivery Address")]
        Shipping,
        [Description("Billing Address")]
        Billing
    }

    public enum CartCurrentStatus
    {
        [Description("Added")]
        Added = 1,
        [Description("Removed")]
        Removed = 2,
        [Description("Purchased")]
        Purchased=3,


    }
    public class EnumHelpers
    {

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);
            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            return default(T);
        }
        public static EnumDetail GetEnumEnumDetailAttribute<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            EnumDetailAttribute[] attributes =
                (EnumDetailAttribute[])fi.GetCustomAttributes(
                typeof(EnumDetailAttribute),
                false);
            if (attributes != null &&
                attributes.Length > 0)
            {
                var model = new EnumDetail
                {
                    LongDescription = attributes[0].LongDescription,
                    ShortDescription = attributes[0].ShortDescription,
                    Code = attributes[0].Code,
                };
                return model;
            }

            else
            {
                var model = new EnumDetail
                {
                    LongDescription = "",
                    ShortDescription = "",
                    Code = -1,
                };
                return model;
            }
        }


    }
    public class EnumDetailAttribute : Attribute
    {
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public int Code { get; set; }
    }
    public class EnumDetail
    {
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public int Code { get; set; }
    }

}

