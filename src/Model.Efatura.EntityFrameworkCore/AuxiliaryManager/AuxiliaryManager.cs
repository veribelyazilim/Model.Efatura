using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Model.Efatura.EDefter;
using Model.Efatura.EntityFrameworkCore;
using Model.Efatura.InvoiceModels;
using Model.Efatura.UpdateModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Model.Efatura.AuxiliaryManager
{
    public class AuxiliaryManager : IAuxiliaryManager, ITransientDependency
    {
        //dbContext işe yararsa connStrye gerek kalmaz, kaldırabiliriz.
        private readonly string _connStr;
        private readonly EfaturaDbContext _dbContext;
        private readonly ModelDbContext _modelDbContext;
        private readonly EDefterDbContext _eDefterDbContext;
        public AuxiliaryManager(IConfiguration config, EfaturaDbContext dbContext, ModelDbContext modelDbContext, EDefterDbContext eDefterDbContext)
        {
            _connStr = config.GetConnectionString("Default");
            _dbContext = dbContext;
            _modelDbContext = modelDbContext;
            _eDefterDbContext = eDefterDbContext;
        }

        public async Task<EDefterResultDto> GetEDefterList(EDefterCo co)
        {
            try
            {
                var connection = _eDefterDbContext.Database.GetDbConnection();

                var transaction =
                    _eDefterDbContext.Database.CurrentTransaction?.GetDbTransaction();

                DynamicParameters p = new DynamicParameters();
                p.Add("@TarihBas", co.StartDate!.Value.Date, DbType.Date);
                p.Add("@TarihBit", co.EndDate!.Value.Date, DbType.Date);
                p.Add("@SonSatirNo", co.PreviousLineNumber, DbType.Int32);
                p.Add("@Kontrol", co.Validate, DbType.Boolean);
                p.Add("@SayfaNo", co.PageNumber, DbType.Int32);
                p.Add("@SayfaBoyutu", co.PageSize, DbType.Int32);
                p.Add("@ToplamSatir", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var lines = await connection.QueryAsync<EDefterLineDto>(
                    "dbo.EDEFTER", p,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 600
                );

                return new EDefterResultDto
                {
                    Items = lines.ToList(),
                    TotalCount = p.Get<int?>("@ToplamSatir") ?? 0
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ProcReturnCombinedDto> GetEfaturaList(InvoiceModelCo co)
        {
            try
            {
                var connection = _modelDbContext.Database.GetDbConnection();

                var transaction =
                    _modelDbContext.Database.CurrentTransaction?.GetDbTransaction();

                ProcReturnCombinedDto combDto = new();
                
                DynamicParameters p = new DynamicParameters();
                p.Add("@PageNumber", co.PageNumber);
                p.Add("@PageSize", co.PageSize);
                p.Add("@CompanyCode", co.CompanyCode);
                p.Add("@UUID", co.UUID);
                
                if (co.StartDate != null)
                {
                    p.Add("@StartDate", co.StartDate);
                }

                if (co.EndDate != null)
                {
                    p.Add("@EndDate", co.EndDate);
                }

                if (co.Sent != null)
                {
                    p.Add("@Sent", co.Sent);
                }

                if (co.NoInvoiceNumber != null)
                {
                    p.Add("@NoInvoiceNumber", co.NoInvoiceNumber);
                }

                if (co.IsEfatura != null)
                {
                    p.Add("@IsEfatura", co.IsEfatura);
                }

                p.Add("@InvoiceNumber", co.InvoiceNumber);

                var lineList = (await connection.QueryAsync<ProcReturnLineDto>(
                    "VRB_EFATURA_LIST", p,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300
                )).ToList();

                //var lineList = (List<ProcReturnLineDto>)result;
                //var lineList = result.ToList();
                string VeritabaniAdi = "ZRHGROUPAS202601";
                string Cid = "ZHG";

                combDto.ProcLines = lineList;
                combDto.TotalCount = lineList.FirstOrDefault()?.TotalCount ?? 0;

                DynamicParameters p2 = new DynamicParameters();
                p2.Add("@database", VeritabaniAdi); // senin parametre nereden geliyorsa
                p2.Add("@cid", Cid);
                
                string sql = @"
                Select 
                    IdentificationId = a.vno,
                    PartyName = a.firmaunvani,
                    TaxSchemeName = a.vdairesi,
                    CountryName = a.ulke,
                    StreetName = IsNull(a.cadde,'') + ' ' + IsNull(a.sokak,'') + ' ' + 'No: ' + IsNull(a.kapino,'') + ' ' + IsNull(a.ilce,'') + ' ' + IsNull(a.sehir,''),
                    ElectronicMail = a.email,
                    WebsiteURI = a.internet_adresi,
                    Telefax = a.fax,
                    Telephone = a.telefon,
                    CityName = a.sehir
                From " + VeritabaniAdi + @".dbo.tnm_firma a
                Where a.cid = @cid
                ";

                var firma = await connection.QueryFirstOrDefaultAsync<CustomerDto>(
                    sql,
                    p2,
                    transaction: transaction,
                    commandType: CommandType.Text
                );

                combDto.ProcCustomer = firma;

                return combDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ProcReturnCombinedDto> GetEfaturaListHt(InvoiceModelCo co)
        {
            try
            {
                var connection = _modelDbContext.Database.GetDbConnection();

                var transaction =
                    _modelDbContext.Database.CurrentTransaction?.GetDbTransaction();

                ProcReturnCombinedDto combDto = new();

                DynamicParameters p = new DynamicParameters();
                p.Add("@PageNumber", co.PageNumber);
                p.Add("@PageSize", co.PageSize);
                p.Add("@CompanyCode", co.CompanyCode);
                p.Add("@UUID", co.UUID);

                if (co.StartDate != null)
                {
                    p.Add("@StartDate", co.StartDate);
                }

                if (co.EndDate != null)
                {
                    p.Add("@EndDate", co.EndDate);
                }

                if (co.Sent != null)
                {
                    p.Add("@Sent", co.Sent);
                }

                if (co.NoInvoiceNumber != null)
                {
                    p.Add("@NoInvoiceNumber", co.NoInvoiceNumber);
                }

                if (co.IsEfatura != null)
                {
                    p.Add("@IsEfatura", co.IsEfatura);
                }

                p.Add("@InvoiceNumber", co.InvoiceNumber);

                var lineList = (await connection.QueryAsync<ProcReturnLineDto>(
                    "VRB_EFATURA_LIST_HT", p,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300
                )).ToList();

                //var lineList = (List<ProcReturnLineDto>)result;
                //var lineList = result.ToList();
                string VeritabaniAdi = "ZRHGROUPAS202601";
                string Cid = "ZHG";

                combDto.ProcLines = lineList;
                combDto.TotalCount = lineList.FirstOrDefault()?.TotalCount ?? 0;

                DynamicParameters p2 = new DynamicParameters();
                p2.Add("@database", VeritabaniAdi);
                p2.Add("@cid", Cid);

                string sql = @"
                Select 
                    IdentificationId = a.vno,
                    PartyName = a.firmaunvani,
                    TaxSchemeName = a.vdairesi,
                    CountryName = a.ulke,
                    StreetName = IsNull(a.cadde,'') + ' ' + IsNull(a.sokak,'') + ' ' + 'No: ' + IsNull(a.kapino,'') + ' ' + IsNull(a.ilce,'') + ' ' + IsNull(a.sehir,''),
                    ElectronicMail = a.email,
                    WebsiteURI = a.internet_adresi,
                    Telefax = a.fax,
                    Telephone = a.telefon,
                    CityName = a.sehir
                From " + VeritabaniAdi + @".dbo.tnm_firma a
                Where a.cid = @cid
                ";

                var firma = await connection.QueryFirstOrDefaultAsync<CustomerDto>(
                    sql,
                    p2,
                    transaction: transaction,
                    commandType: CommandType.Text
                );

                combDto.ProcCustomer = firma;

                return combDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task UpdateInvoiceStatus(UpdateModelCo co)
        {
            try
            {
                var connection = _modelDbContext.Database.GetDbConnection();
                var transaction =
                    _modelDbContext.Database.CurrentTransaction?.GetDbTransaction();

                DynamicParameters p = new DynamicParameters();
                p.Add("@UUID", co.UUID);
                p.Add("@DocumentId", co.DocumentId);
                p.Add("@Durum", co.Status);
                p.Add("@Sent", co.Sent);
                p.Add("@DatabaseName", "ZRHGROUPAS202601");
                await connection.ExecuteAsync(
                    "VRB_EFATURA_GUNCELLE", p,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task UpdateInvoiceStatusHt(UpdateModelCo co)
        {
            var connection = _modelDbContext.Database.GetDbConnection();
            var transaction =
                _modelDbContext.Database.CurrentTransaction?.GetDbTransaction();

            DynamicParameters p = new DynamicParameters();
            p.Add("@UUID", co.UUID);
            p.Add("@DocumentId", co.DocumentId);
            p.Add("@Durum", co.Status);
            p.Add("@Sent", co.Sent);
            p.Add("@DatabaseName", "ZRHGROUPAS202601");
            await connection.ExecuteAsync(
                "VRB_EFATURA_GUNCELLE", p,
                transaction: transaction,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ProcReturnCombinedDto> GetEfaturaListHtTest(InvoiceModelCo co)
        {
            try
            {
                var connection = _modelDbContext.Database.GetDbConnection();

                var transaction =
                    _modelDbContext.Database.CurrentTransaction?.GetDbTransaction();

                ProcReturnCombinedDto combDto = new();

                DynamicParameters p = new DynamicParameters();
                p.Add("@PageNumber", co.PageNumber);
                p.Add("@PageSize", co.PageSize);
                p.Add("@CompanyCode", co.CompanyCode);
                p.Add("@UUID", co.UUID);
                p.Add("@DatabaseName", "test01");

                if (co.StartDate != null)
                {
                    p.Add("@StartDate", co.StartDate);
                }

                if (co.EndDate != null)
                {
                    p.Add("@EndDate", co.EndDate);
                }

                if (co.Sent != null)
                {
                    p.Add("@Sent", co.Sent);
                }

                if (co.NoInvoiceNumber != null)
                {
                    p.Add("@NoInvoiceNumber", co.NoInvoiceNumber);
                }

                if (co.IsEfatura != null)
                {
                    p.Add("@IsEfatura", co.IsEfatura);
                }

                p.Add("@InvoiceNumber", co.InvoiceNumber);

                var lineList = (await connection.QueryAsync<ProcReturnLineDto>(
                    "VRB_EFATURA_LIST_HT", p,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300
                )).ToList();

                //var lineList = (List<ProcReturnLineDto>)result;
                //var lineList = result.ToList();
                string VeritabaniAdi = "test01";
                string Cid = "ZHG";

                combDto.ProcLines = lineList;
                combDto.TotalCount = lineList.FirstOrDefault()?.TotalCount ?? 0;

                DynamicParameters p2 = new DynamicParameters();
                p2.Add("@database", VeritabaniAdi);
                p2.Add("@cid", Cid);

                string sql = @"
                Select 
                    IdentificationId = a.vno,
                    PartyName = a.firmaunvani,
                    TaxSchemeName = a.vdairesi,
                    CountryName = a.ulke,
                    StreetName = IsNull(a.cadde,'') + ' ' + IsNull(a.sokak,'') + ' ' + 'No: ' + IsNull(a.kapino,'') + ' ' + IsNull(a.ilce,'') + ' ' + IsNull(a.sehir,''),
                    ElectronicMail = a.email,
                    WebsiteURI = a.internet_adresi,
                    Telefax = a.fax,
                    Telephone = a.telefon,
                    CityName = a.sehir
                From " + VeritabaniAdi + @".dbo.tnm_firma a
                Where a.cid = @cid
                ";

                var firma = await connection.QueryFirstOrDefaultAsync<CustomerDto>(
                    sql,
                    p2,
                    transaction: transaction,
                    commandType: CommandType.Text
                );

                combDto.ProcCustomer = firma;

                return combDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task UpdateInvoiceStatusHtTest(UpdateModelCo co)
        {
            var connection = _modelDbContext.Database.GetDbConnection();
            var transaction =
                _modelDbContext.Database.CurrentTransaction?.GetDbTransaction();

            DynamicParameters p = new DynamicParameters();
            p.Add("@UUID", co.UUID);
            p.Add("@DocumentId", co.DocumentId);
            p.Add("@Durum", co.Status);
            p.Add("@Sent", co.Sent);
            p.Add("@DatabaseName", "test01");
            await connection.ExecuteAsync(
                "VRB_EFATURA_GUNCELLE", p,
                transaction: transaction,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
