import { Component, Inject, ViewChild } from '@angular/core';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MatSelectionList, MatSelectionListChange, MAT_DIALOG_DATA } from '@angular/material';
import { TreeNode } from 'primeng/components/common/treenode';

import { Permission, RoleDetails } from '../models/role.models';

interface IExpandedNode {
  id: number;
  label: string;
}

@Component({
  selector: 'app-role-form-dialog',
  templateUrl: './role-form-dialog.component.html',
  styleUrls: ['./role-form-dialog.component.scss']
})
export class RoleFormDialogComponent implements OnInit {
  formGroup: FormGroup;
  model: RoleDetails;
  perms: Permission[];
  stagesTree: TreeNode[] = [];
  filteredValues: TreeNode[] = [];
  selectedNodes: TreeNode[] = [];
  selectedPermissions: string[];
  expandedNodes: IExpandedNode[] = [];

  constructor(
    private dialogRef: MatDialogRef<RoleFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private formBuilder: FormBuilder) {
    this.buildForm();
    if (data.model) {
      this.formGroup.patchValue(data.model);
      this.formGroup.get('code').disable();
      this.selectedPermissions = data.model.permissions;
    }
    this.perms = this.data.perms;
  }

  ngOnInit(): void {
    const filter = (value, root) => {
      return root.filter((entry) => {
        if (entry.children && entry.children.length) {
          const result = filter(value, entry.children);
          if (result.length) {
            entry.children = result;

            return result;
          }
        }

        return entry.label.toLowerCase().includes(value.toLowerCase());
      });
    };

    const expand = (data) => {
      for (let entry of data) {
        if (entry.children) {
          entry.expanded = true;

          expand(entry.children);
        }
      }
    };

    const copy = (data) => {
      try {
        return JSON.parse(JSON.stringify(data));
      } catch (error) {
        console.error(error);

        return data.slice();
      }
    };

    this.filteredValues = copy(this.data.stagesTree);
    let previousValue = null;

    this.setSelectedStages();
    this.formGroup.get('filter').valueChanges
      .subscribe(() => {
        const value = this.formGroup.get('filter').value;
        if (value) {
          this.filteredValues = filter(value, copy(this.data.stagesTree));

          if (value.length >= 3) {
            expand(this.filteredValues);
          }
        }

        if (previousValue && previousValue.length > value.length) {
          this.filteredValues = filter(value, copy(this.data.stagesTree));
        }

        const selectedIDs = Array.from(new Set(this.selectedNodes.map((entry) => (entry.data))));
        for (let i = 0; i < this.filteredValues.length; i++) {
          this.setSelectedNode(this.filteredValues[i], selectedIDs);
        }

        previousValue = value;
      });
  }

  setRoleStagesValue(selected: any[]): void {
    const stageIds = Array.from(new Set(selected.map((entry) => entry.data))).filter(id => id > 0);

    this.formGroup.get('roleStages').setValue(stageIds);
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  onSubmit(): void {
    this.dialogRef.close(this.formGroup.getRawValue());
  }

  onPermissionsSelectionChanged(event: MatSelectionListChange): void {
    const permissions = event.source.selectedOptions.selected.map((entry) => entry.value);

    this.formGroup.patchValue({ permissions });
  }

  private buildForm(): void {
    this.formGroup = this.formBuilder.group({
      id: [''],
      nameRu: ['', [Validators.required]],
      nameEn: [''],
      nameKz: [''],
      code: ['', [Validators.required]],
      permissions: ['', Validators.required],
      filter: [''],
      roleStages: ['', [Validators.required]]
    });
  }

  private setSelectedNode(treeNode: TreeNode, ids: number[], parent?: TreeNode): void {
    if (ids.includes(treeNode.data)) {
      treeNode.leaf = true;

      if (treeNode.data > 0) {
        this.selectedNodes.forEach((entry, index) => {
          if (entry.data === treeNode.data && entry.label === treeNode.label) {
            this.selectedNodes.splice(index, 1);
          }
        });

        this.selectedNodes.push(treeNode);
      }

      if (!this.expandedNodes.find((entry) => (entry.id === treeNode.data && entry.label === treeNode.label))) {
        this.expandedNodes.push({
          id: treeNode.data,
          label: treeNode.label
        });
      }

      if (parent) {
        if (this.expandedNodes.find((entry) => (entry.id === treeNode.data && entry.label === treeNode.label))) {
          parent.partialSelected = true;
          parent.expanded = true;
        } else {
          parent.partialSelected = false;
          parent.expanded = false;
        }
      }
    }

    if (treeNode.children) {
      for (let child of treeNode.children) {
        this.setSelectedNode(child, ids, treeNode);
      }
    }
  }

  private setSelectedStages(): void {
    if (this.data.model) {
      for (let i = 0; i < this.filteredValues.length; i++) {
        this.setSelectedNode(this.filteredValues[i], this.data.model.roleStages);
      }
    }
  }
}
