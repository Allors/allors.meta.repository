Path=C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin

xsd bin\debug\Allors.Repository.dll /type:Allors.Design.Meta.MetaSubjectXml
move Schema0.xsd MetaSubject.xsd


xsd bin\debug\Allors.Repository.dll /type:Allors.Design.Meta.MetaTypeXml
move Schema0.xsd MetaType.xsd

xsd bin\debug\Allors.Repository.dll /type:Allors.Design.Meta.MetaRelationXml
move Schema0.xsd MetaRelation.xsd

xsd /?

pause