# Running Locally

## Terraform Commands

### Sandbox

```cmd
terraform init -backend-config="./../tf-backend/sandbox-backend.hcl"
terraform apply -var-file="./../tf-vars/sandbox.tfvars"
```

### Dev

```cmd
terraform init -backend-config="./../tf-backend/dev-backend.hcl"
terraform apply -var-file="./../tf-vars/dev.tfvars"
```
