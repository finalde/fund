drop table if exists fund;
create table if not exists fund(
	id bigserial not null primary key,
	date date not null,
	code text not null,
	short_name text not null,
	name text not null,
	type text not null,
	full_name text not null,
	three_year_increase decimal(25,7),
	one_year_increase decimal(25,7),
	six_month_increase decimal(25,7),
	three_month_increase decimal(25,7),
	one_month_increase decimal(25,7),
	since_inception_increase decimal(25,7),
	size decimal(25,7),
	manager text,
	creation_date date,
	active boolean not null,
	event_type char not null,
	audit_by text not null,
	event_time timestamp not null,
	db_time timestamp default current_timestamp
);
create index ix_Fund_Date on fund(date);
create index ix_Fund_Code on fund(code);

drop table if exists fund_company;
create table if not exists fund_company(
	id bigserial not null primary key,
	date date not null,
	code text not null,
	name text not null,
	creation_date date not null,
	total_funds int not null,
	managing_director text not null,
	abbr text not null,
	asset_under_management decimal(25,7) not null,
	star_rating int not null,
	short_name text not null,
	statistics_date date not null,
	active boolean not null,
	event_type char not null,
	audit_by text not null,
	event_time timestamp not null,
	db_time timestamp default current_timestamp
);
create index ix_FundCompany_Date on fund_company(date);
create unique index ux_FundCompany_Date_Code on fund_company(date, code);

drop table if exists fund_manager;
create table if not exists fund_manager(
	id bigserial not null primary key,
	date date not null,
	code text not null,
	name text not null,
	company_code text not null,
	company_name text not null,
	fund_codes text not null,
	fund_names text not null,
	experience_in_days int not null,
	best_perform_fund_code text,
	best_perform_fund_name text,
	total_asset_under_management decimal(25,7) not null,
	best_fund_return decimal(25,7) not null,
	active boolean not null,
	event_type char not null,
	audit_by text not null,
	event_time timestamp not null,
	db_time timestamp default current_timestamp
);
create index ix_FundManager_Date on fund_manager(date);
create index ix_FundManager_CompanyCode on fund_manager(company_code);
create unique index ux_FundManager_Date_Code on fund_manager(date, code);

drop table if exists index_fund;
create table if not exists index_fund(
	id bigserial not null primary key,
	date date not null,
	code text not null,
	open decimal(25,7),
	high decimal(25,7),
	low decimal(25,7),
	close decimal(25,7),
	pre_close decimal(25,7),
	change decimal(25,7),
	pct_chg decimal(25,7),
	volume decimal(25,7),
	amount decimal(25,7),
	active boolean not null,
	event_type char not null,
	audit_by text not null,
	event_time timestamp not null,
	db_time timestamp default current_timestamp
);
create index ix_IndexFund_Date on index_fund(date);
create index ix_IndexFund_Code on index_fund(code);
create unique index ux_IndexFund_Date_Code on index_fund(date, code);

--drop table if exists fund_detail;
-- create table if not exists fund_detail(
-- 	id bigserial not null primary key,
-- 	fund text not null,
-- 	date date not null,
-- 	unit_value decimal(25,7),
-- 	cumulative_value decimal(25,7),
-- 	daily_increase decimal(25,7),
-- 	pnl_per_ten_thousands_share decimal(25,7),
-- 	anualized_profit_seven_days decimal(25,7),
-- 	creation_status text not null,
-- 	redemption_status text not null,
-- 	dividend text,
-- 	active boolean not null,
-- 	event_type char not null,
-- 	audit_by text not null,
-- 	event_time timestamp not null,
-- 	db_time timestamp default current_timestamp
-- );
-- create index ix_FundDetail_Date on fund_detail(date);
-- create index ix_FundDetail_Fund on fund_detail(fund);
-- create unique index ux_FundDetail_Date_Fund on fund_detail(date, fund);


drop table if exists fund_calculated_data;
create table if not exists fund_calculated_data(
	id bigserial not null primary key,
	date text not null,
	fund text not null,
	inception_date date,
	beta_inception decimal(25,7),
	alpha_inception decimal(25,7),
	treynor_inception decimal(25,7),
	beta_current decimal(25,7),
	alpha_current decimal(25,7),
	treynor_current decimal(25,7),
	active boolean not null,
	event_type char not null,
	audit_by text not null,
	event_time timestamp not null,
	db_time timestamp default current_timestamp
);

drop table if exists shareholder;
create table if not exists shareholder(
	id bigserial not null primary key,
	date text not null,
	fund text not null,
	institution decimal(25,7),
	individual decimal(25,7),
	internal decimal(25,7),
	total_amount decimal(25,7),
	active boolean not null,
	event_type char not null,
	audit_by text not null,
	event_time timestamp not null,
	db_time timestamp default current_timestamp
);
create index ix_Shareholder_Date on shareholder(date);
create index ix_Shareholder_Fund on shareholder(fund);

drop table if exists morning_star;
create table if not exists morning_star(
	id bigserial not null primary key,
	date text not null,
	code text not null,
	name text not null,
	type text,
	three_year_rating int,
	five_year_rating int,
	value_date date,
	unit_value decimal(25,7),
	daily_change decimal(25,7),
	current_year_return decimal(25,7),
	active boolean not null,
	event_type char not null,
	audit_by text not null,
	event_time timestamp not null,
	db_time timestamp default current_timestamp
);
create index ix_MorningStar_Date on morning_star(date);
create index ix_MorningStar_Code on morning_star(code);
create index ix_MorningStar_Name on morning_star(name);


select * from fund_company;
select * from fund_manager;

select  
	f.code,
	f.name, 
	h.institution, 
	d.inception_date,
	d.beta_inception,
	d.alpha_inception,
	treynor_inception,
	d.beta_current,
	d.alpha_current,
	treynor_current,
	s.three_year_rating, 
	s.five_year_rating, 
	f.one_year_increase,
	f.six_month_increase,
	f.three_month_increase, 
	size,
	c.star_rating,
	f.manager,
	m.experience_in_days, 
	c.name,
	m.best_fund_return, 
	h.date as sharedate, 
	h.internal, 
	h.individual
	from fund f
join fund_manager m on m.name = f.manager
join fund_company c on c.short_name = m.company_name
join fund_calculated_data d on d.fund = f.code
left join morning_star s on s.code = f.code
left join (
 select * from (
select row_number() over (partition by fund order by date desc) as rownum, * from shareholder
) as foo where rownum = 1
 ) h on h.fund = f.code
where 
	size >= 200000000 
	and size < 10000000000 
	and f.type in ('混合型','股票型') 
	and m.experience_in_days > 1000
	and m.best_fund_return > 0.7
	and f.six_month_increase > 0.3
	and f.one_year_increase > 0.6 
	and d.beta_inception < 0.7
	and d.beta_current < 0.7
	and c.star_rating > 3
	and (three_year_rating > 3 or three_year_rating is null)
	and (five_year_rating > 3 or five_year_rating is null)
	and institution > 0.1
	and treynor_current > 1
order by d.alpha_current desc;

select * from shareholder;
select * from (
select row_number() over (partition by fund order by date desc) as rownum, * from shareholder
) as foo where rownum = 1

select * from fund_manager where name = '陈平';

select count(*) from shareholder;

select * from morning_star;
select distinct fund from fund_detail where daily_increase = 0 limit 1000;

select * from index_fund;
select date, name from fund_manager group by date, name
having count(*) > 1;
select count(*) from (
select distinct fund from fund_detail
	) foo;

select 1+ daily_increase from fund_Detail where fund = '570008' and daily_increase > 0.05;

select count(*) from fund_detail;
select * from fund_detail where fund = '270009' order by date

select * from fund_calculated_data;



select date, fund from fund_detail group by date, fund
having count(*) > 1;

