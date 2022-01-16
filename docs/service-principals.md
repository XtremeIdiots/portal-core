# [<](index.md) Service Principals (+Permissions)

## Pipeline Service Principal

The pipeline service principal needs to be in the following roles:

* __Application Administrator__ - This is to allow the pipeline to create/manage application registrations.
* __Groups Administrator__ - This is to allow the pipeline to create/manage AAD groups.

---

## SQL Managed Identity

The SQL Managed Identity needs to be in the following roles:

* __Directory Readers__ - This is to allow the SQL Server to query for AAD users when authenticating through AAD.
