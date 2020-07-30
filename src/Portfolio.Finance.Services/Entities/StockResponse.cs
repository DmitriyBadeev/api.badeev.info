using System.Collections.Generic;
using System.Text.Json;

namespace Portfolio.Finance.Services.Entities
{
    // StockResponse myDeserializedClass = JsonConvert.DeserializeObject<StockResponse>(myJsonResponse); 
    public class SECID
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class BOARDID
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class SHORTNAME
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class PREVPRICE
    {
        public string type { get; set; }

    }

    public class LOTSIZE
    {
        public string type { get; set; }

    }

    public class FACEVALUE
    {
        public string type { get; set; }

    }

    public class STATUS
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class BOARDNAME
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class DECIMALS
    {
        public string type { get; set; }

    }

    public class SECNAME
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class REMARKS
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class MARKETCODE
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class INSTRID
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class SECTORID
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class MINSTEP
    {
        public string type { get; set; }

    }

    public class PREVWAPRICE
    {
        public string type { get; set; }

    }

    public class FACEUNIT
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class PREVDATE
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class ISSUESIZE
    {
        public string type { get; set; }

    }

    public class ISIN
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class LATNAME
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class REGNUMBER
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class PREVLEGALCLOSEPRICE
    {
        public string type { get; set; }

    }

    public class PREVADMITTEDQUOTE
    {
        public string type { get; set; }

    }

    public class CURRENCYID
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class SECTYPE
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class LISTLEVEL
    {
        public string type { get; set; }

    }

    public class SETTLEDATE
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class Metadata
    {
        public SECID SECID { get; set; }
        public BOARDID BOARDID { get; set; }
        public SHORTNAME SHORTNAME { get; set; }
        public PREVPRICE PREVPRICE { get; set; }
        public LOTSIZE LOTSIZE { get; set; }
        public FACEVALUE FACEVALUE { get; set; }
        public STATUS STATUS { get; set; }
        public BOARDNAME BOARDNAME { get; set; }
        public DECIMALS DECIMALS { get; set; }
        public SECNAME SECNAME { get; set; }
        public REMARKS REMARKS { get; set; }
        public MARKETCODE MARKETCODE { get; set; }
        public INSTRID INSTRID { get; set; }
        public SECTORID SECTORID { get; set; }
        public MINSTEP MINSTEP { get; set; }
        public PREVWAPRICE PREVWAPRICE { get; set; }
        public FACEUNIT FACEUNIT { get; set; }
        public PREVDATE PREVDATE { get; set; }
        public ISSUESIZE ISSUESIZE { get; set; }
        public ISIN ISIN { get; set; }
        public LATNAME LATNAME { get; set; }
        public REGNUMBER REGNUMBER { get; set; }
        public PREVLEGALCLOSEPRICE PREVLEGALCLOSEPRICE { get; set; }
        public PREVADMITTEDQUOTE PREVADMITTEDQUOTE { get; set; }
        public CURRENCYID CURRENCYID { get; set; }
        public SECTYPE SECTYPE { get; set; }
        public LISTLEVEL LISTLEVEL { get; set; }
        public SETTLEDATE SETTLEDATE { get; set; }

    }

    public class Securities
    {
        public Metadata metadata { get; set; }
        public List<string> columns { get; set; }
        public List<List<object>> data { get; set; }

    }

    public class SECID2
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class BOARDID2
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class BID
    {
        public string type { get; set; }

    }

    public class BIDDEPTH
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class OFFER
    {
        public string type { get; set; }

    }

    public class OFFERDEPTH
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class SPREAD
    {
        public string type { get; set; }

    }

    public class BIDDEPTHT
    {
        public string type { get; set; }

    }

    public class OFFERDEPTHT
    {
        public string type { get; set; }

    }

    public class OPEN
    {
        public string type { get; set; }

    }

    public class LOW
    {
        public string type { get; set; }

    }

    public class HIGH
    {
        public string type { get; set; }

    }

    public class LAST
    {
        public string type { get; set; }

    }

    public class LASTCHANGE
    {
        public string type { get; set; }

    }

    public class LASTCHANGEPRCNT
    {
        public string type { get; set; }

    }

    public class QTY
    {
        public string type { get; set; }

    }

    public class VALUE
    {
        public string type { get; set; }

    }

    public class VALUEUSD
    {
        public string type { get; set; }

    }

    public class WAPRICE
    {
        public string type { get; set; }

    }

    public class LASTCNGTOLASTWAPRICE
    {
        public string type { get; set; }

    }

    public class WAPTOPREVWAPRICEPRCNT
    {
        public string type { get; set; }

    }

    public class WAPTOPREVWAPRICE
    {
        public string type { get; set; }

    }

    public class CLOSEPRICE
    {
        public string type { get; set; }

    }

    public class MARKETPRICETODAY
    {
        public string type { get; set; }

    }

    public class MARKETPRICE
    {
        public string type { get; set; }

    }

    public class LASTTOPREVPRICE
    {
        public string type { get; set; }

    }

    public class NUMTRADES
    {
        public string type { get; set; }

    }

    public class VOLTODAY
    {
        public string type { get; set; }

    }

    public class VALTODAY
    {
        public string type { get; set; }

    }

    public class VALTODAYUSD
    {
        public string type { get; set; }

    }

    public class ETFSETTLEPRICE
    {
        public string type { get; set; }

    }

    public class TRADINGSTATUS
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class UPDATETIME
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class ADMITTEDQUOTE
    {
        public string type { get; set; }

    }

    public class LASTBID
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class LASTOFFER
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class LCLOSEPRICE
    {
        public string type { get; set; }

    }

    public class LCURRENTPRICE
    {
        public string type { get; set; }

    }

    public class MARKETPRICE2
    {
        public string type { get; set; }

    }

    public class NUMBIDS
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class NUMOFFERS
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class CHANGE
    {
        public string type { get; set; }

    }

    public class TIME
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class HIGHBID
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class LOWOFFER
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class PRICEMINUSPREVWAPRICE
    {
        public string type { get; set; }

    }

    public class OPENPERIODPRICE
    {
        public string type { get; set; }

    }

    public class SEQNUM
    {
        public string type { get; set; }

    }

    public class SYSTIME
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class CLOSINGAUCTIONPRICE
    {
        public string type { get; set; }

    }

    public class CLOSINGAUCTIONVOLUME
    {
        public string type { get; set; }

    }

    public class ISSUECAPITALIZATION
    {
        public string type { get; set; }

    }

    public class ISSUECAPITALIZATIONUPDATETIME
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class ETFSETTLECURRENCY
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class VALTODAYRUR
    {
        public string type { get; set; }

    }

    public class TRADINGSESSION
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class Metadata2
    {
        public SECID2 SECID { get; set; }
        public BOARDID2 BOARDID { get; set; }
        public BID BID { get; set; }
        public BIDDEPTH BIDDEPTH { get; set; }
        public OFFER OFFER { get; set; }
        public OFFERDEPTH OFFERDEPTH { get; set; }
        public SPREAD SPREAD { get; set; }
        public BIDDEPTHT BIDDEPTHT { get; set; }
        public OFFERDEPTHT OFFERDEPTHT { get; set; }
        public OPEN OPEN { get; set; }
        public LOW LOW { get; set; }
        public HIGH HIGH { get; set; }
        public LAST LAST { get; set; }
        public LASTCHANGE LASTCHANGE { get; set; }
        public LASTCHANGEPRCNT LASTCHANGEPRCNT { get; set; }
        public QTY QTY { get; set; }
        public VALUE VALUE { get; set; }
        public VALUEUSD VALUE_USD { get; set; }
        public WAPRICE WAPRICE { get; set; }
        public LASTCNGTOLASTWAPRICE LASTCNGTOLASTWAPRICE { get; set; }
        public WAPTOPREVWAPRICEPRCNT WAPTOPREVWAPRICEPRCNT { get; set; }
        public WAPTOPREVWAPRICE WAPTOPREVWAPRICE { get; set; }
        public CLOSEPRICE CLOSEPRICE { get; set; }
        public MARKETPRICETODAY MARKETPRICETODAY { get; set; }
        public MARKETPRICE MARKETPRICE { get; set; }
        public LASTTOPREVPRICE LASTTOPREVPRICE { get; set; }
        public NUMTRADES NUMTRADES { get; set; }
        public VOLTODAY VOLTODAY { get; set; }
        public VALTODAY VALTODAY { get; set; }
        public VALTODAYUSD VALTODAY_USD { get; set; }
        public ETFSETTLEPRICE ETFSETTLEPRICE { get; set; }
        public TRADINGSTATUS TRADINGSTATUS { get; set; }
        public UPDATETIME UPDATETIME { get; set; }
        public ADMITTEDQUOTE ADMITTEDQUOTE { get; set; }
        public LASTBID LASTBID { get; set; }
        public LASTOFFER LASTOFFER { get; set; }
        public LCLOSEPRICE LCLOSEPRICE { get; set; }
        public LCURRENTPRICE LCURRENTPRICE { get; set; }
        public MARKETPRICE2 MARKETPRICE2 { get; set; }
        public NUMBIDS NUMBIDS { get; set; }
        public NUMOFFERS NUMOFFERS { get; set; }
        public CHANGE CHANGE { get; set; }
        public TIME TIME { get; set; }
        public HIGHBID HIGHBID { get; set; }
        public LOWOFFER LOWOFFER { get; set; }
        public PRICEMINUSPREVWAPRICE PRICEMINUSPREVWAPRICE { get; set; }
        public OPENPERIODPRICE OPENPERIODPRICE { get; set; }
        public SEQNUM SEQNUM { get; set; }
        public SYSTIME SYSTIME { get; set; }
        public CLOSINGAUCTIONPRICE CLOSINGAUCTIONPRICE { get; set; }
        public CLOSINGAUCTIONVOLUME CLOSINGAUCTIONVOLUME { get; set; }
        public ISSUECAPITALIZATION ISSUECAPITALIZATION { get; set; }
        public ISSUECAPITALIZATIONUPDATETIME ISSUECAPITALIZATION_UPDATETIME { get; set; }
        public ETFSETTLECURRENCY ETFSETTLECURRENCY { get; set; }
        public VALTODAYRUR VALTODAY_RUR { get; set; }
        public TRADINGSESSION TRADINGSESSION { get; set; }

    }

    public class Marketdata
    {
        public Metadata2 metadata { get; set; }
        public List<string> columns { get; set; }
        public List<List<JsonElement>> data { get; set; }

    }

    public class DataVersion2
    {
        public string type { get; set; }

    }

    public class Seqnum2
    {
        public string type { get; set; }

    }

    public class Metadata3
    {
        public DataVersion2 data_version { get; set; }
        public Seqnum2 seqnum { get; set; }

    }

    public class Dataversion
    {
        public Metadata3 metadata { get; set; }
        public List<string> columns { get; set; }
        public List<List<int>> data { get; set; }

    }

    public class Boardid3
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class Secid3
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }

    }

    public class Metadata4
    {
        public Boardid3 boardid { get; set; }
        public Secid3 secid { get; set; }

    }

    public class MarketdataYields
    {
        public Metadata4 metadata { get; set; }
        public List<string> columns { get; set; }
        public List<object> data { get; set; }

    }

    public class StockResponse
    {
        public Securities securities { get; set; }
        public Marketdata marketdata { get; set; }
        public Dataversion dataversion { get; set; }
        public MarketdataYields marketdata_yields { get; set; }

    }
}
