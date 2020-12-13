export interface FeatureSlim  {
    id: string
    no: string
    description: string
    release?: string
    addedToRoadmap: string,
    lastModified: string,
    editType: string,
    status: string,
    valueHash: string,
    tagCategories?: TagCategory[],
}

export interface Feature extends FeatureSlim  {
    details?: string
    moreInfo?: string
    changes?: ChangeSet[]
}

export interface TagCategory {
    category: string,
    tags: string[]
}

export interface ChangeSet {
    date: string,
    type: string,
    changes: Change[]
}

export interface Change {
    property: string,
    oldValue: string,
    newValue: string,
}


export default Feature;