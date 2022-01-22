# Running Locally

## Terraform Commands

```cmd
terraform init -backend-config="./../tf-backend/sandbox-backend.hcl"
terraform apply -var-file="./../tf-vars/sandbox.tfvars"
```
