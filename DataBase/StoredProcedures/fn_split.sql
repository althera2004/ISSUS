

CREATE function [dbo].[fn_split](
	@str varchar(8000),
	@spliter char(1)
)
returns @returnTable table (item varchar(8000))
as
begin
declare @spliterIndex int
select @str = @str + @spliter

while len(@str) > 0
begin
select @spliterIndex = charindex(@spliter,@str)
if @spliterIndex = 1
insert @returnTable (item)
values (null)
else
insert @returnTable (item)
values (substring(@str, 1, @spliterIndex-1))
select @str = substring(@str, @spliterIndex+1, len(@str)-@spliterIndex)
end
return
end 

