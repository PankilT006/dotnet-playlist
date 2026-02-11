using System;
using WebApi.Controllers.DTO;
using WebApi.Controllers.Models;

namespace WebApi.Controllers.Repository;

public static class StudentRepository
{
    public static List<Student> Students { get; set; } = new List<Student>(){
    new Student{
        id=1,
        StudentName="Hari",
        Email="panki09@gmail.com",
        Address="nkadfuiasgusguasd",
        Admission= DateTime.Parse("2026-02-14T06:03:47.402Z")
    }, new Student
    {
        id=2,
        StudentName="Mari",
        Email="panki09@gmail.com",
        Address="nkadfuiasgusguasd",
        Admission= DateTime.Parse("2026-02-19T06:03:47.402Z")

    },new Student
    {
        id=3,
        StudentName="Jari",
        Email="panki09@gmail.com",
        Address="nkadfuiasgusguasd",
        Admission= DateTime.Parse("2026-02-24T06:03:47.402Z")

    }

    };
}
