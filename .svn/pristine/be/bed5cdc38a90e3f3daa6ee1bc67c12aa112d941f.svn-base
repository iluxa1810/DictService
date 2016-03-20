select t.Project_id,t.Task_id,t.Module_id,t.NAME from dbo.Task t with(nolock)
INNER JOIN dbo.project p with(nolock) ON t.project_id = p.project_id
INNER JOIN dbo.Module m with(nolock)  ON t.Module_id=m.Module_id
where p.state != 0 and m.Name like '%Apex%'